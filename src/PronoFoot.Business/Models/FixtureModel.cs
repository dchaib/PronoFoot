using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class FixtureModel
    {
        public int FixtureId { get; private set; }
        public int DayId { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeamId { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int AwayTeamId { get; set; }
        public int? AwayTeamGoals { get; set; }

        public bool CanBeForecast { get { return this.Date > DateTime.Now; } }

        public FixtureModel(Fixture fixture)
        {
            this.FixtureId = fixture.FixtureId;
            this.DayId = fixture.DayId;
            this.Date = fixture.Date;
            this.HomeTeamId = fixture.HomeTeamId;
            this.HomeTeamGoals = fixture.HomeTeamGoals;
            this.AwayTeamId = fixture.AwayTeamId;
            this.AwayTeamGoals = fixture.AwayTeamGoals;
        }
    }
}
