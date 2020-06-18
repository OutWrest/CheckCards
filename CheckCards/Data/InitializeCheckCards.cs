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
        public async static void ResetDatabase(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
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
                UserName = "asd",
                Name = "name",
                Email = "user@aol.com",
                Answer1 = "a",
                Answer2 = "a"
            };

            Tasks.Add(userManager.CreateAsync(user, "asd").ContinueWith(task =>
                userManager.AddToRoleAsync(user, AuthorizationRoles.User)));

            Task.WaitAll(Tasks.ToArray());
        }
    }
}
