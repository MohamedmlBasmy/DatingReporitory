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
using DatingApp.API.Extentions;
using System.Linq;
using System.Net.Http;

namespace DatingApp.API.Controllers
{
    //[ServiceFilter(typeof(LastActive))]
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
            ResponseAdd re = new ResponseAdd();

            var currentLoggedInUser = await datingRepository.GetUser(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));

            userParams.Id = currentLoggedInUser.Id;

            if (!string.IsNullOrEmpty(currentLoggedInUser.Gender)
                && string.IsNullOrEmpty(userParams.Gender)
                && userParams.Likees == false
                && userParams.Likers == false)
            {
                userParams.Gender = currentLoggedInUser.Gender == "male" ? "female" : "male";
            }
            PagedList<User> users = await datingRepository.GetUsers(userParams);
            Response.AddPaginationHeader(users.PageNumber, users.PageSize, users.TotalCount, users.TotalPages);
            var usersToReturn = mapper.Map<IEnumerable<UserList>>(users);

            re.ResponseBody = usersToReturn;
            return Ok(re);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this.datingRepository.GetUser(id);
            var userToReturn = this.mapper.Map<UserDetail>(user);
            return Ok(userToReturn);
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




        [HttpPost("{id}/like/{recepientId}")]
        public async Task<IActionResult> Like(int id, int recepientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await datingRepository.Getlikes(id, recepientId);
            if (user != null)
            {
                return BadRequest("You cannot like user more than one");
            }

            var like = new Like
            {
                LikeeId = recepientId,
                LikerId = id
            };

            datingRepository.Add<Like>(like);
            await datingRepository.SaveAll();

            return Ok();
        }
    }
}