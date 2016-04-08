using ForumTriage_Web.Models;
using ForumTriage_Web.Services;
using ForumTriage_Web.ViewModels.Questions;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumTriage_Web.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            //read data files into an array of users
            var users = await _context.User.ToListAsync();

            //construct view model
            var viewModel = new QuestionsSearchGetViewModel()
            {
                Users = users
            };

            return View(viewModel);
        }

        //
        // POST: /Home/Questions
        [HttpPost]
        public async Task<IActionResult> Questions(QuestionsSearchPostViewModel search)
        {
            //prepare master question list
            List<StackOverflowQuestionWithDates> questions = new List<StackOverflowQuestionWithDates>();

            //prepare query strings for api call
            var userIdsForQuery = string.Empty;
            if (search.UserIds != null)
            {
                foreach (var userId in search.UserIds)
                {
                    userIdsForQuery += userId + ";";
                }
                userIdsForQuery = userIdsForQuery.TrimEnd(';');
            }

            //get and parse unanswered results
            var apiUrlUnanswered = (string.Format("users/{0}/questions/unanswered?pagesize={1}&order=desc&sort=activity&site=stackoverflow",
                userIdsForQuery,
                Constants.Constants.StackOverflowApiPageSize));
            questions = ParseAPIResults(await StackOverflowAPIService.CallApi(apiUrlUnanswered), questions);

            //get and parse no answer results
            var apiUrlNoAnswers = (string.Format("users/{0}/questions/no-answers?pagesize={1}&order=desc&sort=activity&site=stackoverflow",
                userIdsForQuery,
                Constants.Constants.StackOverflowApiPageSize));
            questions = ParseAPIResults(await StackOverflowAPIService.CallApi(apiUrlNoAnswers), questions);

            //get and parse unaccepted results
            var apiUrlUnaccepted = (string.Format("users/{0}/questions/unaccepted?pagesize={1}&order=desc&sort=activity&site=stackoverflow",
                userIdsForQuery,
                Constants.Constants.StackOverflowApiPageSize));
            questions = ParseAPIResults(await StackOverflowAPIService.CallApi(apiUrlUnaccepted), questions);

            //populate view model
            var filteredQuestions = questions
                .Where(o => o.is_answered != true)
                .Where(o => o.locked_date == 0)
                .Where(o => o.closed_date == 0)
                .ToList();
            var vm = new QuestionsViewModel()
            {
                Questions = filteredQuestions
            };

            return View(vm);
        }






        private List<StackOverflowQuestionWithDates> ParseAPIResults(IList<JToken> results, List<StackOverflowQuestionWithDates> questions)
        {
            foreach (var result in results)
            {
                StackOverflowQuestionWithDates question = JsonConvert.DeserializeObject<StackOverflowQuestionWithDates>(result.ToString());
                question = AddActualDatesToQuestion(question);
                if (questions.Where(o => o.question_id == question.question_id).Count() <= 0) questions.Add(question);
            }
            return questions;
        }

        private StackOverflowQuestionWithDates AddActualDatesToQuestion(StackOverflowQuestionWithDates question)
        {
            //setup epoch time. This is how dates are returns from the StackOverflow API. It is some weird unix thing apparently. It is defined as the number of seconds since midnight (UTC) on 1st January 1970.
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            if (question.closed_date != 0) question.actual_closed_date = epoch.AddSeconds(question.closed_date);
            if (question.locked_date != 0) question.actual_locked_date = epoch.AddSeconds(question.locked_date);
            if (question.last_activity_date != 0) question.actual_last_activity_date = epoch.AddSeconds(question.last_activity_date);
            if (question.creation_date != 0) question.actual_creation_date = epoch.AddSeconds(question.creation_date);
            if (question.last_edit_date != 0) question.actual_last_edit_date = epoch.AddSeconds(question.last_edit_date);

            return question;
        }
    }
}
