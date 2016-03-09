using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.Models
{
    //Class generated at http://json2csharp.com/ from API response JSON
    public class StackOverflowQuestion
    {
        public List<string> tags { get; set; }
        public StackOverflowOwner owner { get; set; }
        public bool is_answered { get; set; }
        public int view_count { get; set; }
        public int favorite_count { get; set; }
        public int down_vote_count { get; set; }
        public int up_vote_count { get; set; }
        public int answer_count { get; set; }
        public int closed_date { get; set; }
        public int score { get; set; }
        public int locked_date { get; set; }
        public int last_activity_date { get; set; }
        public int creation_date { get; set; }
        public int last_edit_date { get; set; }
        public int question_id { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    public class StackOverflowQuestions
    {
        public IEnumerable<StackOverflowQuestion> Questions { get; set; }
    }
}
