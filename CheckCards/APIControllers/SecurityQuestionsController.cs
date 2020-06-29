using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CheckCards.Data;
using CheckCards.Models;
using CheckCards.Models.ViewModels;
using CheckCards.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SecurityQuestionsController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IAServices AServices;
        public SecurityQuestionsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAServices AServices)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.AServices = AServices;
        }


        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var id = User.Claims.ToList<Claim>()[0].Value;

            var user = await userManager.FindByIdAsync(id);
 
            if (user != null)
            {
                if (user.HasAnswers())
                {
                    return new OkResult();
                }
            }
            return new UnauthorizedResult();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SetupSecurityQuestions([FromBody] SecurityQuestionsRequestModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Answer1) || string.IsNullOrWhiteSpace(model.Answer2))
                return new UnauthorizedResult();

            var id = User.Claims.ToList<Claim>()[0].Value;

            var user = await userManager.FindByIdAsync(id);

            if (user != null && !user.HasAnswers())
            {
                user.Answer1 = model.Answer1;
                user.Answer2 = model.Answer2;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new OkResult();
                }
            }
            return new UnauthorizedResult();
        }
    }
}
