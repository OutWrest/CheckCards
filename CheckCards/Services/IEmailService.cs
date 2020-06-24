using CheckCards.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CheckCards.Services
{
    public interface IEmailService : IEmailSender
    {
        public void EmailTwoFactorCode(ApplicationUser user);
        public void EmailPasswordResetCode(ApplicationUser user);
    }
}