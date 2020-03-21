using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }//password salt actes as a key so that we're able to recreate the hash and compare it against the password that the user types in
    }
}