using System;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Value> Value { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // options.UseSqlServer(@"");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
            .HasKey(key => new { key.LikerId, key.LikeeId });

            modelBuilder.Entity<Like>()
            .HasOne(like => like.Likee)
            .WithMany(user => user.Likers)
            //          .HasForeignKey(foreign => foreign.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
            .HasOne(like => like.Liker)
            .WithMany(user => user.Likees)
            //        .HasForeignKey(foreign => foreign.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<Message>().HasKey(x=> new {x.SenderId, x.RecepientId});

            modelBuilder.Entity<Message>()
            .HasOne(x => x.Sender)
            .WithMany(x => x.SentMessages)
            .OnDelete(DeleteBehavior.NoAction);
            // .HasForeignKey(x => x.SenderId)
            // .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
            .HasOne(x => x.Recipient)
            .WithMany(x => x.ReceivedMessages)
            .OnDelete(DeleteBehavior.NoAction);
            // .HasForeignKey(x => x.RecepientId)
            // .OnDelete(DeleteBehavior.Restrict)
        }
    }
}