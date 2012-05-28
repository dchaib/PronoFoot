using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.Security;
using PronoFoot.Business.Models;
using PronoFoot.ViewModels;
using PronoFoot.Models;
using PronoFoot.Models.Competition;

namespace PronoFoot.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly IScoringService scoringService;
        private readonly ICompetitionService competitionService;
        private readonly IClassificationService classificationService;

        public CompetitionController(IUserService userService,
            IFixtureService fixtureService,
            IDayService dayService,
            IForecastService forecastService,
            IScoringService scoringService,
            ICompetitionService competitionService,
            IAuthenticationService authenticationService,
            IClassificationService classificationService)
            : base(userService, authenticationService)
        {
            this.dayService = dayService;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.scoringService = scoringService;
            this.competitionService = competitionService;
            this.classificationService = classificationService;
        }

        public ActionResult Index()
        {
            var competitions = competitionService.GetCompetitions();
            return View(new CompetitionListModel
            {
                Competitions = competitions.Select(x => new PronoFoot.Models.Competition.CompetitionModel { Id = x.CompetitionId, Name = x.Name }).ToList()
            });
        }

        public ActionResult Details(int id)
        {
            var competition = competitionService.GetCompetition(id);

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
            var scores = classificationService.GetUserScoresForCompetition(id);
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

        [ChildActionOnly]
        public ActionResult CurrentCompetitions()
        {
            var competitions = competitionService.GetCurrentCompetitions();
            var users = userService.GetUsers();

            var viewModels = new List<CompetitionOverviewModel>();
            foreach (var competition in competitions)
            {
                var viewModel = new CompetitionOverviewModel
                {
                    Id = competition.CompetitionId,
                    Name = competition.Name
                };

                var nextFixture = fixtureService.GetNextFixture(competition.CompetitionId);
                if (nextFixture != null)
                    viewModel.NextFixture = new CompetitionOverviewModel.FixtureOverviewModel() { DateTime = nextFixture.Date, DayId = nextFixture.DayId };

                var scores = classificationService.GetUserScoresForCompetition(competition.CompetitionId);
                viewModel.Scores = scores.Select(x => new UserScoreViewModel
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
                });

                viewModels.Add(viewModel);
            }

            return PartialView(viewModels);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Create()
        {
            return View(new PronoFoot.Models.Competition.CompetitionModel());
        }

        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public ActionResult Create(PronoFoot.Models.Competition.CompetitionModel model)
        {
            if (ModelState.IsValid)
            {
                var competition = model.ToEntity();

                int competitionId = competitionService.Create(competition);

                return RedirectToAction("Details", new { id = competitionId });
            }

            return View(model);
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int id)
        {
            var competition = competitionService.GetCompetition(id);

            if (competition == null)
            {
                return new HttpNotFoundResult("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            return View(competition.ToModel());
        }

        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public ActionResult Edit(PronoFoot.Models.Competition.CompetitionModel model)
        {
            if (ModelState.IsValid)
            {
                var competition = model.ToEntity();

                competitionService.Update(competition);

                return RedirectToAction("Details", new { id = competition.CompetitionId });
            }

            return View(model);
        }
    }
}
