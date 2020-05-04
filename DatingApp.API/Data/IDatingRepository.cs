using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Like> Getlikes(int id, int recepientId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetUserMessages(MessageParams messageParams);

        Task<IEnumerable<Message>> GetThread(int userId, int recepientId);

    }
}