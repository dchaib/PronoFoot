using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.ViewModels;
using PronoFoot.Security;
using PronoFoot.Business.Models;

namespace PronoFoot.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly IScoringService scoringService;
        private readonly ICompetitionService competitionService;

        public HomeController(IUserService userService,
            IFixtureService fixtureService,
            IDayService dayService,
            IForecastService forecastService,
            IScoringService scoringService,
            ICompetitionService competitionService,
            IAuthenticationService authenticationService)
            : base(userService, authenticationService)
        {
            this.dayService = dayService;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.scoringService = scoringService;
            this.competitionService = competitionService;
        }

        public ActionResult Index()
        {
            int competitionId = 2;
            CompetitionModel competition = competitionService.GetCompetition(competitionId);

            if (competition == null)
            {
                return HttpNotFound("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            IDictionary<int, int> forecastCounts;
            if (Request.IsAuthenticated)
                forecastCounts = forecastService.GetForecastCountByDayForCompetitionUser(competitionId, this.CurrentUser.UserId);
            else
                forecastCounts = new Dictionary<int, int>();

            var days = dayService.GetDaysForCompetition(competitionId);
            var fixtures = fixtureService.GetFixturesForCompetition(competitionId);
            var scores = userService.GetUserScoresForCompetition(competitionId);
            var users = userService.GetUsers();

            return View(new HomeViewModel
            {
                Competition = new CompetitionViewModel { CompetitionId = competition.CompetitionId, Name = competition.Name },
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
            var examples = new List<ScoringExample>();
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 2, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 2, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 3, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 3, ForecastAwayGoals = 2 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 4, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 4, ForecastAwayGoals = 3 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 2 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 0, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 2, ForecastAwayGoals = 2 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 3, ForecastAwayGoals = 3 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 2 });

            return View(examples.Select(x => new HomeRulesExampleViewModel
            {
                ScoreHomeGoals = x.ScoreHomeGoals,
                ScoreAwayGoals = x.ScoreAwayGoals,
                ForecastHomeGoals = x.ForecastHomeGoals,
                ForecastAwayGoals = x.ForecastAwayGoals,
                Score = scoringService.GetScore(x.ScoreHomeGoals, x.ScoreAwayGoals, x.ForecastHomeGoals, x.ForecastAwayGoals)
            }));
        }

        private class ScoringExample
        {
            public int ScoreHomeGoals { get; set; }
            public int ScoreAwayGoals { get; set; }
            public int ForecastHomeGoals { get; set; }
            public int ForecastAwayGoals { get; set; }
        }
    }
}
