using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.Security;
using PronoFoot.Business.Models;
using PronoFoot.ViewModels;

namespace PronoFoot.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly IScoringService scoringService;
        private readonly ICompetitionService competitionService;

        public CompetitionController(IUserService userService,
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

        public ActionResult Details (int id)
        {
            CompetitionModel competition = competitionService.GetCompetition(id);

            if (competition == null)
            {
                return HttpNotFound("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            IDictionary<int, int> forecastCounts;
            if (Request.IsAuthenticated)
                forecastCounts = forecastService.GetForecastCountByDayForCompetitionUser(id, this.CurrentUser.UserId);
            else
                forecastCounts = new Dictionary<int, int>();

            var days = dayService.GetDaysForCompetition(id);
            var fixtures = fixtureService.GetFixturesForCompetition(id);
            var scores = userService.GetUserScoresForCompetition(id);
            var users = userService.GetUsers();
            var dayViewModels = days.Select(x => new DayViewModel
                {
                    DayId = x.DayId,
                    Name = x.Name,
                    Date = x.Date,
                    ForecastMadeByCurrentUser = (fixtures.Count(y => y.DayId == x.DayId) == (forecastCounts.ContainsKey(x.DayId) ? forecastCounts[x.DayId] : 0)),
                    CanBeForecast = fixtures.Any(y => y.DayId == x.DayId && y.CanBeForecast)
                });

            return View(new CompetitionDetailsViewModel
            {
                Competition = new CompetitionViewModel { CompetitionId = competition.CompetitionId, Name = competition.Name },
                PreviousDays = dayViewModels.Where(x => !x.CanBeForecast),
                NextDays = dayViewModels.Where(x => x.CanBeForecast),
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
    }
}
