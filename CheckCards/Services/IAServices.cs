using CheckCards.Data;
using System.Threading.Tasks;

namespace CheckCards.Services
{
    public interface IAServices
    {
        public Task SendTwoFactorCodeAsync(ApplicationUser user);
        public bool ValidateTwoFactorCodeAsync(ApplicationUser user, string code);
        public Task SendPasswordResetAsync(ApplicationUser user);
    }
}