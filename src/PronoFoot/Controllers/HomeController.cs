using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Data.Model;
using PronoFoot.Data;
using PronoFoot.Data.EntityFramework.Repositories;
using PronoFoot.Data.EntityFramework;
using PronoFoot.Business.Contracts;
using PronoFoot.ViewModels;

namespace PronoFoot.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICompetitionRepository competitionRepository;
        private readonly IDayRepository dayRepository;

        public HomeController(IUserService userService, ICompetitionRepository competitionRepository, IDayRepository dayRepository)
            : base(userService)
        {
            this.competitionRepository = competitionRepository;
            this.dayRepository = dayRepository;
        }

        public ActionResult Index()
        {
            int competitionId = 2;
            Competition competition = competitionRepository.GetCompetition(competitionId);

            if (competition == null)
            {
                return HttpNotFound("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            competition.Days = dayRepository.GetDays(competition.CompetitionId).ToList();
            var scores = UserService.GetUserScoresForCompetition(competitionId);
            var users = UserService.GetUsers();

            return View(new HomeViewModel
            {
                Competition = competition,
                Scores = scores.Select(x => new UserScoreViewModel
                {
                    UserId = x.UserId,
                    UserName = users.First(y => y.UserId == x.UserId).Name,
                    Score = x.Score,
                    NumberOfExactForecasts = x.NumberOfExactForecasts,
                    NumberOfCloseForecasts = x.NumberOfCloseForecasts,
                    NumberOfForecastsWithExactDifference = x.NumberOfForecastsWithExactDifference,
                    NumberOfCorrect1N2Forecasts = x.NumberOfCorrect1N2Forecasts,
                    NumberOfWrongForecasts = x.NumberOfWrongForecasts,
                    PercentageOfScoringForecasts = x.PercentageOfScoringForecasts
                })
            });
        }

        public ActionResult Rules()
        {
            return View();
        }
    }
}
