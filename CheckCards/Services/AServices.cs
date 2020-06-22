using CheckCards.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckCards.Services
{
    public class AServices : IAServices
    {


        private ApplicationDbContext dbContext;
        private IEmailService emailService;
        private static Random random = new Random();
        private UserManager<ApplicationUser> userManager;
        public AServices(ApplicationDbContext dbContext, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.emailService = emailService;
            this.userManager = userManager;
        }

        public async Task SendTwoFactorCodeAsync(ApplicationUser user)
        {
            int code = random.Next(0, 999999);
            user.TwoFactorCode = code.ToString("000000");
            user.TwoFactorCodeDateTime = DateTime.Now;
            await userManager.UpdateAsync(user);
            emailService.EmailTwoFactorCode(user);
        }

        public bool ValidateTwoFactorCodeAsync(ApplicationUser user, string code)
        {
            if (user.TwoFactorEnabled && user.TwoFactorCodeDateTime != null && !string.IsNullOrEmpty(user.TwoFactorCode))
            {
                TimeSpan codeTimeSpan = DateTime.Now - user.TwoFactorCodeDateTime;
                if (codeTimeSpan <= TimeSpan.FromMinutes(5))
                {
                    if (code == user.TwoFactorCode)
                    {
                        user.TwoFactorCode = "";
                        userManager.UpdateAsync(user);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
