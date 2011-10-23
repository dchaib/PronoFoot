using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public class Fixture
    {
        public int FixtureId { get; set; }
        public int DayId { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeamId { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int AwayTeamId { get; set; }
        public int? AwayTeamGoals { get; set; }

        public ICollection<Forecast> Forecasts { get; set; }
    }
}
