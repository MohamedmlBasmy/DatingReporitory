using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext Context;
        public DatingRepository(DataContext context)
        {
            this.Context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            Context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            Context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user =  await Context.Users.Include(user => user.Photos).FirstOrDefaultAsync(user => user.Id == id);
            return  user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  Context.Users.Include(x => x.Photos).AsQueryable();

            var minDOB = DateTime.Now.AddYears(-userParams.MaxAge - 1);
            var maxDOB = DateTime.Now.AddYears(-userParams.MinAge);

            users = users.Where(user => user.Id != userParams.Id);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                users = users.Where(user => user.DateOfBirth <= maxDOB && user.DateOfBirth >= minDOB);
            }

            if (!string.IsNullOrEmpty(userParams.Gender))
            {
                users = users.Where(user => user.Gender == userParams.Gender);
            }

            if (userParams.Likees)
            {
                var userLikers = await GetLikesIds(userParams.Id, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likers)
            {
                var userLikees = await GetLikesIds(userParams.Id, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(userParams.SortType))
            {
                switch (userParams.SortType)
                {
                    case "created":
                        users.OrderByDescending(or => or.Created);
                        break;

                    default:
                        users.OrderByDescending(order => order.LastActive);
                        break;
                }
            }
            return await PagedList<User>.CreatePaging(users, userParams.PageSize, userParams.PageNumber);
        }

        private async Task<IEnumerable<int>> GetLikesIds(int userId, bool Likers)
        {
            var user = await Context.Users.Include(n => n.Likees).Include(n => n.Likers)
            .FirstOrDefaultAsync(x => x.Id == userId);
            if (Likers)
            {
                var likersIds = user.Likers.Where(x => x.LikeeId == userId).Select(x => x.LikerId);
                return likersIds;
            }
            else
            {
                var likeesIds = user.Likees.Where(x => x.LikerId == userId).Select(x => x.LikeeId);
                return likeesIds;
            }
        }

        public async Task<bool> SaveAll()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<Like> Getlikes(int id, int recepientId)
        {
            return await Context.Like.FirstOrDefaultAsync(x => x.LikerId == id && x.LikeeId == recepientId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await Context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<Message>> GetUserMessages(MessageParams messageParams)
        {
            var messages = Context.Messages.Include(x => x.Sender).Include(x => x.Recipient).AsQueryable();
            switch (messageParams.MessageType)
            {
                case "Outbox":
                    messages = messages.Where(x => x.SenderId == messageParams.Id && x.SenderDeleted == false);
                    break;
                case "Inbox":
                    messages = messages.Where(x => x.RecipientId == messageParams.Id && x.RecipientDeleted == false);
                    break;
                default:
                    messages = messages.Where(x => x.RecipientId == messageParams.Id && x.IsRead == false && x.SenderDeleted == false);
                    break;
            }
            messages = messages.OrderBy(x => x.MessageSent);

            return await PagedList<Message>.CreatePaging(messages, messageParams.PageSize, messageParams.PageNumber);
        }

        public async Task<IEnumerable<Message>> GetThread(int userId, int recepientId)
        {
            var messages = await Context.Messages
            .Include(x => x.Sender)
            .Include(x => x.Recipient)
            .Where(
                x => x.SenderId == userId && x.RecipientId == recepientId &&  x.RecipientDeleted == false
                || x.RecipientId == userId && x.SenderId == recepientId && x.SenderDeleted == false).ToListAsync();
            return messages;
        }
    }
}