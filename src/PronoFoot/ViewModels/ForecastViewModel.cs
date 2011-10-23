using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Business.Models;

namespace PronoFoot.ViewModels
{
    public class ForecastViewModel
    {
        public int? ForecastId { get; set; }
        public FixtureModel Fixture { get; private set; }
        public int FixtureId { get; set; }
        public TeamModel HomeTeam { get; set; }
        public TeamModel AwayTeam { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }

        public ForecastViewModel()
        {
        }

        public ForecastViewModel(FixtureModel fixture, TeamModel homeTeam, TeamModel awayTeam)
        {
            this.Fixture = fixture;
            this.FixtureId = fixture.FixtureId;
            this.HomeTeam = homeTeam;
            this.AwayTeam = awayTeam;
        }

        public ForecastViewModel(ForecastModel forecast, FixtureModel fixture, TeamModel homeTeam, TeamModel awayTeam)
            : this(fixture, homeTeam, awayTeam)
        {
            this.ForecastId = forecast.ForecastId > 0 ? (int?)forecast.ForecastId : null;
            this.HomeTeamGoals = forecast.ForecastId > 0 ? (int?)forecast.HomeTeamGoals : null;
            this.AwayTeamGoals = forecast.ForecastId > 0 ? (int?)forecast.AwayTeamGoals : null;
        }
    }
}