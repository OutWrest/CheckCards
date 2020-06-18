using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckCards.Models
{
    public class AuthorizationRoles
    {

        public const string User = "User";
        public const string Administrator = "Administrator";

        public static List<string> AllRoles = new List<string> { Administrator, User };
    }
}