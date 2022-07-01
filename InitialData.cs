using ApiTest.Authentication;
using ApiTest.Context;
using ApiTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest
{
    public static class InitialData
    {
        public static void Seed(this UserContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                string password = BCrypt.Net.BCrypt.HashPassword("admin");
                dbContext.Users.Add(new User
                {
                    Username = "Pieter Ebbers",
                    Password = password,
                    Email = "pieter.ebbers99@gmail.com"
                });

                dbContext.SaveChanges();
            }
        }

        public static void Seed(this TodoContext dbContext)
        {
            if (!dbContext.TodoEntries.Any())
            {
                dbContext.TodoEntries.Add(new TodoItem
                {
                    UserId = 3,
                    Description = "Create second database",
                    DueDate = DateTime.Today.AddDays(1),
                    IsDone = false
                });

                dbContext.SaveChanges();
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("Pieter").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Pieter";
                user.Email = "pieter.ebbers99@gmail.com";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "admin").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, UserRoles.Admin).Wait();
                    userManager.AddToRoleAsync(user, UserRoles.User).Wait();
                }
            } 

            if (userManager.FindByNameAsync("BlackHunt").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "BlackHunt";
                user.Email = "blackhunt@live.nl";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "admin").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, UserRoles.Admin).Wait();
                    userManager.AddToRoleAsync(user, UserRoles.User).Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(UserRoles.Admin).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = UserRoles.Admin;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(UserRoles.User).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = UserRoles.User;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(UserRoles.Guest).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = UserRoles.Guest;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
    }
}
