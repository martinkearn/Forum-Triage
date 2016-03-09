using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.Models
{
    public class StackOverflowQuestionWithDates : StackOverflowQuestion
    {
        public DateTime actual_closed_date { get; set; }
        public DateTime actual_locked_date { get; set; }
        public DateTime actual_last_activity_date { get; set; }
        public DateTime actual_creation_date { get; set; }
        public DateTime actual_last_edit_date { get; set; }
    }
}
