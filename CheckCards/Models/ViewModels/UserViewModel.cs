using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckCards.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}