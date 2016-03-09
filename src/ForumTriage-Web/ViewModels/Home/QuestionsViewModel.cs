using ForumTriage_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.ViewModels.Home
{
    public class QuestionsViewModel
    {
        public IEnumerable<StackOverflowQuestionWithDates> Questions { get; set; }
    }
}
