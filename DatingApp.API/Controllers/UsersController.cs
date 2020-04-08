using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helper;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LastActive))]
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository datingRepository;
        private readonly IMapper mapper;
        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.datingRepository = datingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await datingRepository.GetUsers(userParams);

            var usersToReturn = mapper.Map<IEnumerable<UserList>>(users);
            // HttpContext.Response.Headers.Add("customHeader","something");
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this.datingRepository.GetUser(id);
            var userToReturn = this.mapper.Map<UserDetail>(user);
            return BadRequest(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdate UserForUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            else
            {
                var userFromRepo = await datingRepository.GetUser(id);
                mapper.Map(UserForUpdate, userFromRepo);

                if (await this.datingRepository.SaveAll())
                {
                    NoContent();
                }
            }
            return Ok(UserForUpdate);
        }
    }
}