using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.Models
{
    public class StackOverflowUser
    {
        public StackOverflowBadgeCounts badge_counts { get; set; }
        public int account_id { get; set; }
        public bool is_employee { get; set; }
        public int last_modified_date { get; set; }
        public int last_access_date { get; set; }
        public int age { get; set; }
        public int reputation_change_year { get; set; }
        public int reputation_change_quarter { get; set; }
        public int reputation_change_month { get; set; }
        public int reputation_change_week { get; set; }
        public int reputation_change_day { get; set; }
        public int reputation { get; set; }
        public int creation_date { get; set; }
        public string user_type { get; set; }
        public int user_id { get; set; }
        public int accept_rate { get; set; }
        public string location { get; set; }
        public string website_url { get; set; }
        public string link { get; set; }
        public string profile_image { get; set; }
        public string display_name { get; set; }
    }
}
