using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using CheckCards.Services;
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
        private IAServices AServices;

        public ResetPasswordController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAServices AServices)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.AServices = AServices;
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

                    TimeSpan codeTimeSpan = DateTime.Now - user.PasswordResetCodeDateTime;
                    if (codeTimeSpan <= TimeSpan.FromMinutes(5))
                    {
                        user.TwoFactorCode = "";
                    }

                    await userManager.RemovePasswordAsync(user);
                    await userManager.AddPasswordAsync(user, model.NewPassword);
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return new OkResult();
                }
            }
            return new UnauthorizedResult();
        }
    }

    public class ChangePasswordRequestViewModel
    {
        public string NewPassword { get; set; }
    }
}