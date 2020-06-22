using CheckCards.Data;
using CheckCards.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]")]
    [ApiController]
    public class MultiFactorController : ControllerBase
    {
        private IConfiguration configuration;
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IAServices AServices;
        public MultiFactorController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAServices AServices, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.AServices = AServices;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponseViewModel>> Post([FromBody] MultiFactorRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.MultiFactorValue))
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
                                new Claim(ClaimTypes.Role,role)
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
            return new UnauthorizedResult();
        }
    }

    public class MultiFactorRequestViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MultiFactorValue { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string Token { get; set; }
    }
}