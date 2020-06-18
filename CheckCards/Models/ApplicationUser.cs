using Microsoft.AspNetCore.Identity;


namespace CheckCards.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }

        [PersonalData]
        public string Name { get; set; }
        public bool HasSetupChallengeQuestions()
        {
            return Answer1 != null && Answer2 != null;
        }
        public bool VerifyChallengeAnswers(string answer1, string answer2)
        {
            return Answer1.Trim().ToLower() == answer1.Trim().ToLower() && Answer2.Trim().ToLower() == answer2.Trim().ToLower();
        }
        public bool CQVerified { get; set; }
        public bool TFVerified { get; set; }
    }
}