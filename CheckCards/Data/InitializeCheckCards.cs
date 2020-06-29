using CheckCards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckCards.Data
{
    public class InitializeCheckCards
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async static void ResetDatabase(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            Random random = new Random();

            // Deletes all past users and roles from previous session

            List<Task> Tasks = new List<Task>();
            foreach (ApplicationUser user_to_be_deleted in context.Users)
            {
                Tasks.Add(userManager.DeleteAsync(user_to_be_deleted));

            }

            foreach (IdentityRole role_to_be_deleted in roleManager.Roles.ToList())
            {
                Tasks.Add(roleManager.DeleteAsync(role_to_be_deleted));
            }

            Task.WaitAll(Tasks.ToArray());

            // Create Roles

            foreach (string role in AuthorizationRoles.AllRoles)
            {
                Tasks.Add(roleManager.CreateAsync(new IdentityRole(role)));
            }

            // Create a normal user

            ApplicationUser user = new ApplicationUser
            {
                UserName = "user",
                Name = "name",
                Email = "user@6mails.com",
                TwoFactorEnabled = true
            };

            Tasks.Add(userManager.CreateAsync(user, "asd").ContinueWith(task =>
                userManager.AddToRoleAsync(user, AuthorizationRoles.User)));

            Task.WaitAll(Tasks.ToArray());

            // Create a secure user 

            ApplicationUser user2 = new ApplicationUser
            {
                UserName = "user" + random.Next(0, 99).ToString("00"),
                Name = "name" + random.Next(0, 99).ToString("00"),
                Email = "user2@6mails.com",
                Answer1 = RandomString(5),
                Answer2 = RandomString(5),
                TwoFactorEnabled = true
            };

            Tasks.Add(userManager.CreateAsync(user2, RandomString(5)).ContinueWith(task =>
                userManager.AddToRoleAsync(user2, AuthorizationRoles.User)));

            // Create an admin user

            ApplicationUser admin = new ApplicationUser
            {
                UserName = "admin",
                Name = "admin",
                Email = "admin@6mails.com",
                Answer1 = "asd",
                Answer2 = "asd",
                TwoFactorEnabled = true
            };

            Tasks.Add(userManager.CreateAsync(admin, "asd").ContinueWith(task =>
                userManager.AddToRoleAsync(admin, AuthorizationRoles.Administrator)));

            // Create a secure admin user

            ApplicationUser admin2 = new ApplicationUser
            {
                UserName = "admin" + random.Next(0, 99).ToString("00"),
                Name = "admin" + random.Next(0, 99).ToString("00"),
                Email = "admin2@6mails.com",
                Answer1 = RandomString(5),
                Answer2 = RandomString(5),
                TwoFactorEnabled = true
            };

            Tasks.Add(userManager.CreateAsync(admin2, RandomString(5)).ContinueWith(task =>
                userManager.AddToRoleAsync(admin2, AuthorizationRoles.Administrator)));

            Task.WaitAll(Tasks.ToArray());

            Task.WaitAll(Tasks.ToArray());

            Tasks.Add(userManager.CreateAsync(admin, "asd").ContinueWith(task =>
                userManager.AddToRoleAsync(admin, AuthorizationRoles.Administrator)));

            Tasks.Add(userManager.CreateAsync(admin2, RandomString(5)).ContinueWith(task =>
                userManager.AddToRoleAsync(admin2, AuthorizationRoles.Administrator)));

            Task.WaitAll(Tasks.ToArray());

            var users = userManager.Users.ToList();
        }
    }
}
