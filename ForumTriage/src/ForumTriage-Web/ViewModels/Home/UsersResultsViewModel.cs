using ForumTriage_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.ViewModels.Home
{
    public class UsersViewModel
    {
        public IEnumerable<StackOverflowUser> Users { get; set; }
    }
}
