using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.ViewModels;

namespace PronoFoot.Models.Competition
{
    public class CompetitionOverviewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FixtureOverviewModel NextFixture { get; set; }
        public IEnumerable<UserScoreViewModel> Scores { get; set; }

        public class FixtureOverviewModel
        {
            public DateTime DateTime { get; set; }
            public int DayId { get; set; }
        }
    }
}