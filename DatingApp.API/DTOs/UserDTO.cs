using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}