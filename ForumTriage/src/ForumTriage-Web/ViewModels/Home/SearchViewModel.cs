using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.ViewModels.Home
{
    public class SearchViewModel
    {
        public IEnumerable<string> Owners { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
