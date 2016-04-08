using ForumTriage_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.ViewModels.StackOverflowUsers
{
    public class StackOverflowUsersViewModel
    {
        public IEnumerable<StackOverflowUser> Users { get; set; }
    }
}
