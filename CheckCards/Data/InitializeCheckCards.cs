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
                UserName = "target",
                Name = "name",
                Email = "target@aol.com"
            };

            Tasks.Add(userManager.CreateAsync(user, "asd").ContinueWith(task =>
                userManager.AddToRoleAsync(user, AuthorizationRoles.User)));

            // Create an admin user

            ApplicationUser admin = new ApplicationUser
            {
                UserName = "targetadmin",
                Name = "admin",
                Email = "target@admin.com"
            };

            Tasks.Add(userManager.CreateAsync(admin, "asda").ContinueWith(task =>
                userManager.AddToRoleAsync(admin, AuthorizationRoles.Administrator)));

            Task.WaitAll(Tasks.ToArray());

            Task.WaitAll(Tasks.ToArray());

            var users = userManager.Users.ToList();
        }
    }
}
