using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.Models
{
    public class User
    {
        public string Name { get; set; }
        public string StackOverflowUserId { get; set; }

        public string Organisation { get; set; }
    }
}
