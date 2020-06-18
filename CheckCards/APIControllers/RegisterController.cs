using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CheckCards.Data;
using CheckCards.Models;
using CheckCards.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        private const string Failed = "Operation failed";
        private const string Succeeded = "Operation successful";


        public RegisterController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        private bool CheckResult(string Name, string Username, string Email, string Password)
        {
            List<string> checkList = new List<string>
            {
                Name,
                Username,
                Email,
                Password
            };

            foreach (string item in checkList)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    return false;
                }
            }

            return true;
        }


        [HttpPut]
        public async Task<ActionResult<ResponseStatusViewModel>> Put(RegisterRequestViewModel model)
        {
            ResponseStatusViewModel res = new ResponseStatusViewModel();

            if (CheckResult(model.Name, model.Username, model.Email, model.Password))
            {
                res.Result = false;
                res.Message = Failed;

                return res;
            }

            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                Email = model.Email,
                UserName = model.Username,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                res.Result = false;
                res.Message = Failed;

                return res;
            }

            await userManager.AddToRoleAsync(user, AuthorizationRoles.User);

            res.Result = true;
            res.Message = Succeeded;

            return new OkObjectResult(res);

        }

    }
}
