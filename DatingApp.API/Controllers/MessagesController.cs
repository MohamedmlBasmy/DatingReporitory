using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helper;
using DatingApp.API.Extentions;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    //[ServiceFilter(typeof(LastActive))]
    [Authorize]
    [Route("api/users/{userId}/[Controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDatingRepository _datingRepository;

        public MessagesController(IDatingRepository datingRepository, IMapper mapper)
        {
            _datingRepository = datingRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            var currentLoggedInUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentLoggedInUser != userId)
            {
                return Unauthorized();
            }
            var message = await _datingRepository.GetMessage(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, [FromQuery]MessageParams messageParams)
        {
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != userId)
            {
                return Unauthorized();
            }
            messageParams.Id = userId;

            var messages = await _datingRepository.GetUserMessages(messageParams);

            var messagetoReturn = _mapper.Map<IEnumerable<MessageForReturn>>(messages);

            Response.AddPaginationHeader(messages.PageNumber, messageParams.PageSize, messages.TotalCount, messages.TotalPages);

            return Ok(messagetoReturn);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetThread(int userId, int recipientId)
        {
            if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) != userId)
            {
                return Unauthorized();
            }
            var messageFromRepo = await _datingRepository.GetThread(userId, recipientId);
            var messageForReturn = _mapper.Map<IEnumerable<MessageForReturn>>(messageFromRepo);
            return Ok(messageForReturn);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, [FromBody]MessageForCreate messageForCreate)
        {
            var sender = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (sender != userId)
            {
                return Unauthorized();
            }

            messageForCreate.SenderId = userId;
            var recepientUser = await _datingRepository.GetUser(messageForCreate.RecipientId);

            if (recepientUser == null)
            {
                return BadRequest("Recepient User Cannot Be Found");
            }

            var createdMessage = _mapper.Map<Message>(messageForCreate);

            _datingRepository.Add<Message>(createdMessage);

            if (await _datingRepository.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageForReturn>(createdMessage);

                messageToReturn.RecipientKnownAs = createdMessage.Recipient.KnownAs;
                //messageToReturn.SenderKnownAs =  createdMessage.Sender.KnownAs;

                return CreatedAtRoute("GetMessage", new { userId = userId, id = createdMessage.Id }, messageToReturn);
            }

            throw new Exception("Creating the message failed on the server");
        } 

        [HttpPost("{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId, int userId)
        {
            var messageFromRepo = await _datingRepository.GetMessage(messageId);
            if (messageFromRepo != null && messageFromRepo.SenderId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                messageFromRepo.SenderDeleted = true;
            }

            if (messageFromRepo != null && messageFromRepo.RecipientId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                messageFromRepo.RecipientDeleted = true;
            }
            _datingRepository.Delete(messageFromRepo);

            if (await _datingRepository.SaveAll())
            {
                return NoContent();
            }
            throw new Exception("Error Deleting Message");
        }

        [HttpPost("{messageId}/read")]
        public async Task<IActionResult> MarkAsRead(int messageId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _datingRepository.GetMessage(messageId);
            
            messageFromRepo.IsRead = true;
            messageFromRepo.DateRead = DateTime.Now;

            await _datingRepository.SaveAll();

            return NoContent();
        }
    }
}