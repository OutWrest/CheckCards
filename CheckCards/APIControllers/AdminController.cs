using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using CheckCards.Models;
using CheckCards.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CheckCards.APIControllers
{
    [Route("/api/v0.999/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AuthorizationRoles.Administrator)]
    public class AdminController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;

        private const string Failed = "Operation failed";
        private const string Succeeded = "Operation successful";

        public AdminController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            List<ApplicationUser> userslist = userManager.Users.ToList<ApplicationUser>();

            AdminResponseViewModel res = new AdminResponseViewModel(userslist);

            return Ok(res);
        }

        [HttpGet("[action]")]
        public IEnumerable<UserViewModel> GetUsers()
        {
            List<UserViewModel> response = new List<UserViewModel>();

            foreach (var user in userManager.Users.ToList<ApplicationUser>())
            {
                response.Add(new UserViewModel
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email
                });
            }


            return response;
        }

        [HttpPost("[action]/{id?}")]
        public new ActionResult User(string id)
        {
            if (id.Equals("1"))
                return Ok("hi");

            ResponseStatusViewModel res = new ResponseStatusViewModel();

            res.Result = false;
            res.Message = Failed;

            return new BadRequestObjectResult(res);
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