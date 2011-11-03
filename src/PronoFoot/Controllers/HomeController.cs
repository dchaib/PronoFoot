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
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly ICompetitionRepository competitionRepository;

        public HomeController(IUserService userService, IFixtureService fixtureService, IDayService dayService, IForecastService forecastService, ICompetitionRepository competitionRepository)
            : base(userService)
        {
            this.dayService = dayService;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.competitionRepository = competitionRepository;
        }

        public ActionResult Index()
        {
            int competitionId = 2;
            Competition competition = competitionRepository.GetCompetition(competitionId);

            if (competition == null)
            {
                return HttpNotFound("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            IDictionary<int, int> forecastCounts;
            if (Request.IsAuthenticated)
                forecastCounts = forecastService.GetForecastCountByDayForCompetitionUser(competitionId, this.CurrentUserId);
            else
                forecastCounts = new Dictionary<int, int>();

            var days = dayService.GetDaysForCompetition(competitionId);
            var fixtures = fixtureService.GetFixturesForCompetition(competitionId);
            var scores = UserService.GetUserScoresForCompetition(competitionId);
            var users = UserService.GetUsers();

            return View(new HomeViewModel
            {
                Competition = competition,
                Days = days.Select(x => new DayViewModel
                {
                    DayId = x.DayId,
                    Name = x.Name,
                    Date = x.Date,
                    ForecastMadeByCurrentUser = (fixtures.Count(y => y.DayId == x.DayId) == (forecastCounts.ContainsKey(x.DayId) ? forecastCounts[x.DayId] : 0)),
                    CanBeForecast = fixtures.Any(y => y.DayId == x.DayId && y.CanBeForecast)
                }),
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
