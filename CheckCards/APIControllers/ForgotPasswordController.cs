using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using CheckCards.Models.ViewModels;
using CheckCards.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]")]
    [ApiController]
    public class ForgotPasswordController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        private IAServices AServices;

        private static string Failed = "Login Failed.";

        public ForgotPasswordController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAServices AServices)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.AServices = AServices;
        }
        private bool CheckResult(string Name, string Username, string Email)
        {
            List<string> checkList = new List<string>
            {
                Name,
                Username,
                Email
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


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ResetPasswordRequestViewModel model)
        {
            if (!CheckResult(model.Name, model.Username, model.Email))
                return new UnauthorizedResult();

            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                // Check user creditentals 

                if (model.Name.Trim().Equals(user.Name) && model.Email.Trim().Equals(user.Email))
                {
                    await AServices.SendPasswordResetAsync(user);
                    return new OkResult();
                }
            }
            return new UnauthorizedResult();
        }
    }

}