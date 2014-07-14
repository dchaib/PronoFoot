using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public class Forecast
    {
        public int ForecastId { get; set; }
        public int FixtureId { get; set; }
        public int UserId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public Rating? Rating { get; set; }
        public double? Score { get; set; }
    }
}
