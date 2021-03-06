﻿using PronoFoot.Business.Contracts;
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
        private readonly ICompetitionService competitionService;
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly IScoringService scoringService;
        private readonly IEditionService editionService;
        private readonly IClassificationService classificationService;
        private readonly ITeamStandingService teamStandingService;

        public EditionController(ICompetitionService competitionService,
            IUserService userService,
            IFixtureService fixtureService,
            IDayService dayService,
            IForecastService forecastService,
            IScoringService scoringService,
            IEditionService editionService,
            IAuthenticationService authenticationService,
            IClassificationService classificationService,
            ITeamStandingService teamStandingService)
            : base(userService, authenticationService)
        {
            this.competitionService = competitionService;
            this.dayService = dayService;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.scoringService = scoringService;
            this.editionService = editionService;
            this.classificationService = classificationService;
            this.teamStandingService = teamStandingService;
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

            var competition = competitionService.GetCompetition(edition.CompetitionId);
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
                Edition = new EditionViewModel
                {
                    EditionId = edition.EditionId,
                    Name = edition.Name,
                    CompetitionName = competition.Name,
                    HasTeamClassification = competition.HasTeamClassification
                },
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

        [ChildActionOnly]
        [OutputCache(Duration = 100)]
        public ActionResult TeamStandings(int editionId)
        {
            var standings = teamStandingService.GetTeamStandings(editionId);
            return PartialView(standings);
        }
    }
}
