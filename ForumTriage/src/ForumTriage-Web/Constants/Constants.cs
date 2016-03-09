using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.Constants
{
    public class Constants
    {
        public const int StackOverflowApiPageSize = 100;
        public const string StackOverflowApiRootUrl = "https://api.stackexchange.com/2.2/";
        public DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
