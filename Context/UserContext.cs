using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Context
{
    using ApiTest.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserContext
        : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(nameof(User.Id));

            modelBuilder.Entity<User>(user =>
            {
                user.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}