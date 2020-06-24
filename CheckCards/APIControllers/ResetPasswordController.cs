using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]/{id?}")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;

        public ResetPasswordController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ChangePasswordRequestViewModel model, string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(model.NewPassword))
            {
                return new UnauthorizedResult();
            }

            foreach (var user in userManager.Users.ToList())
            {
                if (user.PasswordResetCode.Equals(id.Trim()))
                {
                    await userManager.RemovePasswordAsync(user);
                    await userManager.AddPasswordAsync(user, model.NewPassword);


                    return new OkResult();
                }
            }

            return new NotFoundResult();
        }
    }

    public class ChangePasswordRequestViewModel
    {
        public string NewPassword { get; set; }
    }
}