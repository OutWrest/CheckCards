using Microsoft.AspNetCore.Identity;
using System;

namespace CheckCards.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        public string TwoFactorCode { get; set; }
        public DateTime TwoFactorCodeDateTime { get; set; }
    }
}