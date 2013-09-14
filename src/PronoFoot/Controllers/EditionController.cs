using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Models.Edition;
using PronoFoot.Security;
using PronoFoot.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PronoFoot.Controllers
{
    public class EditionController : BaseController
    {
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly IScoringService scoringService;
        private readonly IEditionService editionService;
        private readonly IClassificationService classificationService;

        public EditionController(IUserService userService,
            IFixtureService fixtureService,
            IDayService dayService,
            IForecastService forecastService,
            IScoringService scoringService,
            IEditionService editionService,
            IAuthenticationService authenticationService,
            IClassificationService classificationService)
            : base(userService, authenticationService)
        {
            this.dayService = dayService;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.scoringService = scoringService;
            this.editionService = editionService;
            this.classificationService = classificationService;
        }

        public ActionResult Details(int id)
        {
            EditionModel edition = editionService.GetEdition(id);

            if (edition == null)
            {
                return HttpNotFound("Il n'y a pas d'édition correspondant à cet identifiant");
            }

            IDictionary<int, int> forecastCounts;
            if (Request.IsAuthenticated)
                forecastCounts = forecastService.GetForecastCountByDayForEditionUser(id, this.CurrentUser.UserId);
            else
                forecastCounts = new Dictionary<int, int>();

            var days = dayService.GetDaysForCompetition(id);
            var fixtures = fixtureService.GetFixturesForEdition(id);
            var scores = classificationService.GetUserScoresForEdition(id);
            var users = userService.GetUsers();
            var dayViewModels = days.Select(x => new DayViewModel
            {
                DayId = x.DayId,
                Name = x.Name,
                Date = x.Date,
                Coefficient = x.Coefficient,
                Score = 0,
                ForecastMadeByCurrentUser = (fixtures.Count(y => y.DayId == x.DayId) == (forecastCounts.ContainsKey(x.DayId) ? forecastCounts[x.DayId] : 0)),
                CanBeForecast = fixtures.Any(y => y.DayId == x.DayId && y.CanBeForecast)
            });

            return View(new EditionDetailsViewModel
            {
                Edition = new EditionViewModel { EditionId = edition.EditionId, Name = edition.Name },
                PreviousDays = dayViewModels.Where(x => !x.CanBeForecast),
                NextDays = dayViewModels.Where(x => x.CanBeForecast),
                Scores = scores.Select(x => new UserScoreViewModel
                {
                    UserId = x.Key,
                    UserName = users.First(y => y.UserId == x.Key).Name,
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
        
        //[ChildActionOnly]
        //public ActionResult CurrentEditions()
        //{
        //    var editions = editionService.GetCurrentEditions();
        //    var users = userService.GetUsers();

        //    var viewModels = new List<EditionOverviewModel>();
        //    foreach (var edition in editions)
        //    {
        //        var viewModel = new EditionOverviewModel
        //        {
        //            Id = edition.EditionId,
        //            Name = edition.Name
        //        };

        //        var nextFixture = fixtureService.GetNextFixture(edition.EditionId);
        //        if (nextFixture != null)
        //            viewModel.NextFixture = new CompetitionOverviewModel.FixtureOverviewModel() { DateTime = nextFixture.Date, DayId = nextFixture.DayId };

        //        var scores = classificationService.GetUserScoresForEdition(edition.EditionId);
        //        viewModel.Scores = scores.Select(x => new UserScoreViewModel
        //        {
        //            UserId = x.Key,
        //            UserName = users.First(y => y.UserId == x.Key).Name,
        //            Score = x.Score,
        //            NumberOfExactForecasts = x.NumberOfExactForecasts,
        //            NumberOfCloseForecasts = x.NumberOfCloseForecasts,
        //            NumberOfForecastsWithExactDifference = x.NumberOfForecastsWithExactDifference,
        //            NumberOfCorrect1N2Forecasts = x.NumberOfCorrect1N2Forecasts,
        //            NumberOfWrongForecasts = x.NumberOfWrongForecasts,
        //            PercentageOfScoringForecasts = x.PercentageOfScoringForecasts
        //        });

        //        viewModels.Add(viewModel);
        //    }

        //    return PartialView(viewModels);
        //}

    }
}
