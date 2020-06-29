using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckCards.Controllers
{
    public class ResetPasswordController : Controller
    {
        private UserManager<ApplicationUser> userManager;

        public ResetPasswordController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Route("{controller}/{id?}")]
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new UnauthorizedResult();
            }

            foreach (var user in userManager.Users.ToList())
            {
                if (user.PasswordResetCode != null && user.PasswordResetCode.Equals(id.Trim())) 
                {
                    ViewBag.id = id;
                    return View();
                }
            }

            return new UnauthorizedResult();
        }
    }
}