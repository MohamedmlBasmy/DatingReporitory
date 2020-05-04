using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _config = config;
            this.mapper = mapper;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            if (await _repo.UsersExists(userDTO.Username))
            {
                return BadRequest("User Exists");
            }
            userDTO.Username = userDTO.Username.ToLower();
            var userToCreate = this.mapper.Map<User>(userDTO);

            var createdUser = await _repo.Register(userToCreate, userDTO.Password);

            var userToReturn = this.mapper.Map<User>(createdUser);

            return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO userDto)
        {
            var IsUserExist = await _repo.Login(userDto.Username, userDto.Password);
            if (IsUserExist == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var claimsIdentity = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, IsUserExist.Id.ToString()),
                    new Claim(ClaimTypes.Name, userDto.Username),
                };
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials,
                    Subject = new ClaimsIdentity(claimsIdentity)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
        }
    }
}