using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Models
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _dataContext { get; set; }
        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;

        }

        public async Task<User> Login(string username, string password)
        {
            var loggedinUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(loggedinUser == null){
                return null;
            }
            if(!VerifyPasswordHash(password, loggedinUser.PasswordHash, loggedinUser.PasswordSalt)){
                return null;
            }
            return loggedinUser;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var pas = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var computedHash = pas.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for (int i = 0; i < computedHash.Length; i++)
               {
                   if (computedHash[i] != passwordHash[i])
                   {
                       return false;
                   }
               }
               return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            using (var pas = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = pas.Key;
                passwordHash = pas.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var pas = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = pas.Key;
                passwordHash = pas.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UsersExists(string username)
        {
             if(await _dataContext.Users.AnyAsync(x=>x.Username == username)){
                 return true;
             }
            else return false;
        }
    }
}