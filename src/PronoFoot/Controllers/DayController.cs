﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Data;
using PronoFoot.Business.Contracts;
using PronoFoot.Models;
using PronoFoot.ViewModels;
using PronoFoot.Business.Models;

namespace PronoFoot.Controllers
{
    public class DayController : BaseController
    {
        private readonly IDayServices dayServices;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly ITeamService teamServices;

        public DayController(IUserService userService,
                             IDayServices dayServices,
                             IFixtureService fixtureService,
                             IForecastService forecastService,
                             ITeamService teamServices)
            : base(userService)
        {
            this.dayServices = dayServices;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.teamServices = teamServices;
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var currentUser = this.CurrentUser;
            var day = dayServices.GetDay(id);
            var fixtures = fixtureService.GetFixturesForDay(id);
            var forecasts = forecastService.GetForecastsForDay(id);
            var teams = teamServices.GetTeamsForCompetition(day.CompetitionId);
            return View(new DayDetailsViewModel
            {
                Day = day,
                Fixtures = fixtures.ToList(),
                Teams = teams.ToList(),
                Forecasts = forecasts.ToList()
            });
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int id)
        {
            var day = dayServices.GetDay(id);
            var fixtures = fixtureService.GetFixturesForDay(id);
            var teams = teamServices.GetTeamsForCompetition(day.CompetitionId);

            if (day == null)
            {
                return new HttpNotFoundResult("Il n'y a pas de journée correspondant à cet identifiant");
            }

            return View(new DayEditViewModel
                {
                    DayName = day.Name,
                    Day = new DayFormViewModel
                    {
                        DayId = day.DayId,
                        Date = day.Date,
                        Name = day.Name,
                        Fixtures = fixtures.Select(x => new FixtureViewModel(x, teams)).ToList()
                    }
                });
        }

        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public ActionResult Edit(int id, DayFormViewModel dayForm)
        {
            var day = new DayModel
            {
                DayId = id,
                Date = dayForm.Date,
                Name = dayForm.Name
            };
            
            var fixtures = new List<FixtureModel>();
            foreach (var fixture in dayForm.Fixtures)
            {
                fixtures.Add(new FixtureModel
                {
                    DayId = id,
                    FixtureId = fixture.FixtureId,
                    Date = fixture.Date,
                    HomeTeamId = fixture.HomeTeamId,
                    AwayTeamId = fixture.AwayTeamId,
                    HomeTeamGoals = fixture.HomeTeamGoals,
                    AwayTeamGoals = fixture.AwayTeamGoals
                });
            }

            dayServices.Update(day, fixtures);

            return RedirectToAction("Details", new { id = id });
        }

        [Authorize]
        public ActionResult Forecast(int id)
        {
            var currentUser = this.CurrentUser;
            var day = dayServices.GetDay(id);
            var fixtures = fixtureService.GetFixturesForDay(id).ToList();
            var forecasts = forecastService.GetForecastsForDayUser(id, this.CurrentUserId).ToList();
            var teams = teamServices.GetTeamsForCompetition(day.CompetitionId).ToList();

            var forecastViewModels = new List<ForecastViewModel>();
            foreach (var fixture in fixtures)
            {
                var homeTeam = teams.First(x => x.TeamId == fixture.HomeTeamId);
                var awayTeam = teams.First(x => x.TeamId == fixture.AwayTeamId);
                var forecast = forecasts.SingleOrDefault(f => f.FixtureId == fixture.FixtureId);
                var vm = forecast == null ? new ForecastViewModel(fixture, homeTeam, awayTeam) : new ForecastViewModel(forecast, fixture, homeTeam, awayTeam);
                forecastViewModels.Add(vm);
            }

            return View(new DayForecastViewModel
            {
                Day = day,
                Teams = teams.ToList(),
                Forecasts = forecastViewModels
            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Forecast(int id, IEnumerable<ForecastViewModel> forecasts)
        {
            var models = new List<ForecastModel>();
            var forecastIdsToDelete = new List<int>();
            foreach (var forecast in forecasts)
            {
                if (forecast.HomeTeamGoals.HasValue && forecast.AwayTeamGoals.HasValue)
                {
                    models.Add(new ForecastModel
                    {
                        FixtureId = forecast.FixtureId,
                        UserId = this.CurrentUserId,
                        HomeTeamGoals = forecast.HomeTeamGoals.Value,
                        AwayTeamGoals = forecast.AwayTeamGoals.Value
                    });
                }
                else if (forecast.ForecastId.HasValue && forecast.ForecastId.Value > 0)
                {
                    forecastIdsToDelete.Add(forecast.ForecastId.Value);
                }
            }
            forecastService.SaveForecasts(models);
            forecastService.DeleteForecasts(forecastIdsToDelete);

            return RedirectToAction("Details", new { id = id });
        }
    }
}
