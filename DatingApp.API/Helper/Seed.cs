using System.Collections.Generic;
using System.IO;
using System.Text;
using DatingApp.API.Data;
using Newtonsoft.Json;

namespace DatingApp.API.Models
{
    public class Seed
    {
        public static void SeedUsers(DataContext dataContext)
        {
            var allUsersTxt = File.ReadAllText(@"C:\M basmy\Project\DatingApp.API\Helper\UsersSeed.json");
            var users = JsonConvert.DeserializeObject<List<User>>(allUsersTxt);
            foreach (var user in users)
            {
                byte[] passwordSalt, passwordHash;
                using (var pass = new System.Security.Cryptography.HMACSHA512())
                {
                    passwordHash = pass.ComputeHash(Encoding.UTF8.GetBytes("password"));
                    passwordSalt = pass.Key;
                }
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();
                
                dataContext.Users.Add(user);
            }
            dataContext.SaveChanges();
        }
    }
}