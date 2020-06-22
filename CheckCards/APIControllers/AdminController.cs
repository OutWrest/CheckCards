using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using CheckCards.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CheckCards.APIControllers
{
    [Route("api/v0.999/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AuthorizationRoles.Administrator)]
    public class AdminController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;

        public AdminController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public ActionResult Get()
        {
            List<ApplicationUser> userslist = userManager.Users.ToList<ApplicationUser>();

            AdminResponseViewModel res = new AdminResponseViewModel(userslist);

            return Ok(res);
        }
    }
    public class AdminResponseViewModel
    {
        public List<List<String>> Users { get; set; }

        public AdminResponseViewModel(List<ApplicationUser> AUsers)
        {
            Users = new List<List<String>>();
            foreach (ApplicationUser user in AUsers)
            {
                Users.Add(new List<String> {
                    user.UserName,
                    user.Name,
                    user.Email
                });
            }
        }
    }
}