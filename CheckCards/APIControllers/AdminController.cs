using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("/api/v0.999/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AuthorizationRoles.Administrator)]
    public class AdminController : ControllerBase
    {
        private SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager;
        private IAServices AServices;

        private const string Failed = "Operation failed";
        private const string Succeeded = "Operation successful";

        public AdminController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAServices AServices)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.AServices = AServices;
    }
        private bool CheckResult(string Id, string Name, string Username, string Email)
        {
            List<string> checkList = new List<string>
            {
                Id,
                Name,
                Username,
                Email
            };

            foreach (string item in checkList)
            {
                if (string.IsNullOrWhiteSpace(item) && item.Length >= 3)
                {
                    return false;
                }
            }

            return true;
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            return Ok();
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
        public async Task<ActionResult<ResponseStatusViewModel>> SaveChanges(UserViewModel model, string id)
        {
            ResponseStatusViewModel res = new ResponseStatusViewModel();

            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$");
            var emailMatch = emailRegex.Match(model.Email); 

            if (!CheckResult(model.Id, model.Name, model.UserName, model.Email) && emailMatch.Success && id.Equals(model.Id.Trim()))
            {
                res.Result = false;
                res.Message = Failed;

                return new BadRequestObjectResult(res);
            }

            var user = await userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                user.Name = model.Name.Trim();
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    res.Result = true;
                    res.Message = Succeeded;

                    return new OkObjectResult(res);
                }
            }

            res.Result = false;
            res.Message = Failed;

            return new BadRequestObjectResult(res);
        }

        [HttpPost("[action]/{id?}")]
        public async Task<ActionResult<ResponseStatusViewModel>> ResetPassword(UserViewModel model, string id)
        {
            ResponseStatusViewModel res = new ResponseStatusViewModel();

            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$");
            var emailMatch = emailRegex.Match(model.Email);

            if (!CheckResult(model.Id, model.Name, model.UserName, model.Email) && emailMatch.Success && id.Equals(model.Id.Trim()))
            {
                res.Result = false;
                res.Message = Failed;

                return new BadRequestObjectResult(res);
            }

            var user = await userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                await AServices.SendPasswordResetAsync(user);
                
                res.Result = true;
                res.Message = Succeeded;
                return new OkObjectResult(res);
            }

            res.Result = false;
            res.Message = Failed;

            return new BadRequestObjectResult(res);
        }

        [HttpPost("[action]/{id?}")]
        public async Task<ActionResult<ResponseStatusViewModel>> DeleteUser(UserViewModel model, string id)
        {
            ResponseStatusViewModel res = new ResponseStatusViewModel();

            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$");
            var emailMatch = emailRegex.Match(model.Email);

            if (!CheckResult(model.Id, model.Name, model.UserName, model.Email) && emailMatch.Success && id.Equals(model.Id.Trim()))
            {
                res.Result = false;
                res.Message = Failed;

                return new BadRequestObjectResult(res);
            }

            var user = await userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    res.Result = true;
                    res.Message = Succeeded;

                    return new OkObjectResult(res);
                }
            }

            res.Result = false;
            res.Message = Failed;

            return new BadRequestObjectResult(res);
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

        [HttpPut("[action]/{id?}")]
        public async Task<ActionResult<ResponseStatusViewModel>> CreateUser(UserViewModel model, string id)
        {
            ResponseStatusViewModel res = new ResponseStatusViewModel();

            Regex emailRegex = new Regex(@"^\S+@\S+\.\S+$");
            var emailMatch = emailRegex.Match(model.Email);

            if (!CheckResult(model.Id, model.Name, model.UserName, model.Email) && emailMatch.Success)
            {
                res.Result = false;
                res.Message = Failed;

                return new BadRequestObjectResult(res);
            }

            var user = new ApplicationUser()
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                res.Result = true;
                res.Message = Succeeded;
                return new OkObjectResult(res);
            }

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