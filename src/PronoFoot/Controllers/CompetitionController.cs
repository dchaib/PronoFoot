using AutoMapper.QueryableExtensions;
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
using PronoFoot.Models.Edition;
using PronoFoot.Models.Competition;
using System.Data.Entity;
using PronoFoot.Data.Model;

namespace PronoFoot.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly ICompetitionService competitionService;
        private readonly IEditionService editionService;
        private readonly IFixtureService fixtureService;
        private readonly IClassificationService classificationService;
        private readonly DbContext dbContext;

        public CompetitionController(IUserService userService,
            ICompetitionService competitionService,
            IEditionService editionService,
            IFixtureService fixtureService,
            IClassificationService classificationService,
            IAuthenticationService authenticationService,
            DbContext dbContext)
            : base(userService, authenticationService)
        {
            this.competitionService = competitionService;
            this.editionService = editionService;
            this.fixtureService = fixtureService;
            this.classificationService = classificationService;
            this.dbContext = dbContext;
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

            var editions = editionService.GetEditions(competition.CompetitionId);

            return View(new CompetitionDetailsViewModel
            {
                Competition = competition,
                Editions = editions
            });
        }

        [ChildActionOnly]
        public ActionResult CurrentCompetitions()
        {
            var minDate = DateTime.Today.AddMonths(-3);
            var maxDate = DateTime.Today.AddMonths(2);

            var editions = dbContext
                .Set<Edition>()
                .Where(x => (x.FirstFixtureDate < DateTime.Today && x.LastFixtureDate > DateTime.Today)
                    || (x.FirstFixtureDate > DateTime.Today && x.FirstFixtureDate < maxDate)
                    || (x.LastFixtureDate < DateTime.Today && x.LastFixtureDate > minDate))
                .Project().To<EditionOverviewModel>()
                .ToArray();

            var users = userService.GetUsers();

            foreach (var edition in editions)
            {
                var nextFixture = fixtureService.GetNextFixture(edition.Id);
                if (nextFixture != null)
                    edition.NextFixture = new EditionOverviewModel.FixtureOverviewModel() { DateTime = nextFixture.Date, DayId = nextFixture.DayId };

                var scores = classificationService.GetUserScoresForEdition(edition.Id);
                edition.Scores = scores.Select(x => new UserScoreViewModel
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
                });
            }

            return PartialView(new ActiveEditionsViewModel
            {
                CurrentEditions = editions.Where(x => x.FirstFixtureDate < DateTime.Today && x.LastFixtureDate > DateTime.Today),
                UpcomingEditions = editions.Where(x => x.FirstFixtureDate > DateTime.Today),
                RecentEditions = editions.Where(x => x.LastFixtureDate < DateTime.Today)
            });
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