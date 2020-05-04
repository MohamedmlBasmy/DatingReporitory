using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; } //password salt actes as a key so that we're able to recreate the hash and compare it against the password that the user types in
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual  ICollection<Photo> Photos { get; set; }
        public virtual  ICollection<Like> Likers { get; set; }
        public virtual  ICollection<Like> Likees { get; set; }

        public virtual  ICollection<Message> ReceivedMessages { get; set; }
        public virtual  ICollection<Message> SentMessages { get; set; }
    }
}