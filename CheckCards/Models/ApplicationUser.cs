using Microsoft.AspNetCore.Identity;
using System;

namespace CheckCards.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public string Answer1 { get; set; }
        [PersonalData]
        public string Answer2 { get; set; }
        public string TwoFactorCode { get; set; }
        public DateTime TwoFactorCodeDateTime { get; set; }

        public string PasswordResetCode { get; set; }
        public DateTime PasswordResetCodeDateTime { get; set; }

        public bool VerifyAnswers(string answer1, string answer2)
        {
            return Answer1.Trim().ToLower() == answer1.Trim().ToLower() && Answer2.Trim().ToLower() == answer2.Trim().ToLower();
        }

        public bool HasAnswers()
        {
            return !string.IsNullOrEmpty(Answer1) && !string.IsNullOrEmpty(Answer2);
        }
    }
}