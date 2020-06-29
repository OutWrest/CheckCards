using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
        private IConfiguration configuration;
        public SecurityQuestionsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAServices AServices, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.AServices = AServices;
            this.configuration = configuration;
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
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseViewModel>> Post([FromBody] SecurityQuestionRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.MultiFactorValue) || string.IsNullOrWhiteSpace(model.Answer1) || string.IsNullOrWhiteSpace(model.Answer2))
            {
                return new UnauthorizedResult();
            }

            ApplicationUser user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    if (AServices.ValidateTwoFactorCodeAsync(user, model.MultiFactorValue))
                    {
                        if (user.HasAnswers())
                        {
                            if (user.VerifyAnswers(model.Answer1, model.Answer2))
                            {
                                IList<string> roles = await userManager.GetRolesAsync(user);
                                string role = "";
                                if (roles.Contains("Administrator"))
                                    role = "Administrator";
                                else if (roles.Contains("User"))
                                    role = "User";

                                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtKey"]));
                                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                                {
                                    Subject = new ClaimsIdentity(new Claim[]
                                    {
                                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                                        new Claim(ClaimTypes.Role, role)
                                    }),
                                    Expires = DateTime.UtcNow.AddDays(7),
                                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                                };
                                SecurityToken securityToken = handler.CreateToken(tokenDescriptor);
                                LoginResponseViewModel responseModel = new LoginResponseViewModel();
                                responseModel.Token = handler.WriteToken(securityToken);
                                return new OkObjectResult(responseModel);
                            }
                        }
                    }

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

    public class SecurityQuestionRequestViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MultiFactorValue { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
    }
}
