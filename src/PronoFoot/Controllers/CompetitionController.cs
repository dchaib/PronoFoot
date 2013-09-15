using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.Security;
using PronoFoot.Business.Models;
using PronoFoot.ViewModels;
using PronoFoot.Models.Edition;

namespace PronoFoot.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly ICompetitionService competitionService;
        private readonly IEditionService editionService;
        private readonly IFixtureService fixtureService;
        private readonly IClassificationService classificationService;

        public CompetitionController(IUserService userService,
            ICompetitionService competitionService,
            IEditionService editionService,
            IFixtureService fixtureService,
            IClassificationService classificationService,
            IAuthenticationService authenticationService)
            : base(userService, authenticationService)
        {
            this.competitionService = competitionService;
            this.editionService = editionService;
            this.fixtureService = fixtureService;
            this.classificationService = classificationService;
        }

        public ActionResult Details(int id)
        {
            CompetitionModel competition = competitionService.GetCompetition(id);

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
            var competitions = competitionService.GetCompetitions();
            var users = userService.GetUsers();

            var viewModels = new List<EditionOverviewModel>();
            foreach (var competition in competitions)
            {
                var editions = editionService.GetEditions(competition.CompetitionId);
                foreach (var edition in editions)
                {
                    var viewModel = new EditionOverviewModel
                    {
                        Id = edition.EditionId,
                        Name = edition.Name,
                        CompetitionId = competition.CompetitionId,
                        CompetitionName = competition.Name
                    };

                    var nextFixture = fixtureService.GetNextFixture(edition.EditionId);
                    if (nextFixture != null)
                        viewModel.NextFixture = new EditionOverviewModel.FixtureOverviewModel() { DateTime = nextFixture.Date, DayId = nextFixture.DayId };

                    var scores = classificationService.GetUserScoresForEdition(edition.EditionId);
                    viewModel.Scores = scores.Select(x => new UserScoreViewModel
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

                    viewModels.Add(viewModel);
                }
            }

            return PartialView(viewModels);
        }
    }
}
