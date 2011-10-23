using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class ForecastModel
    {
        public int ForecastId { get; private set; }
        public int UserId { get; set; }
        public int FixtureId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public double? Score { get; set; }

        public ForecastModel()
        {
        }

        public ForecastModel(Forecast forecast)
        {
            this.ForecastId = forecast.ForecastId;
            this.UserId = forecast.UserId;
            this.FixtureId = forecast.FixtureId;
            this.HomeTeamGoals = forecast.HomeTeamGoals;
            this.AwayTeamGoals = forecast.AwayTeamGoals;
            this.Score = forecast.Score;
        }
    }
}
