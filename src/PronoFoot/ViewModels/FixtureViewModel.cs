using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Business.Models;
using System.Web.Mvc;

namespace PronoFoot.ViewModels
{
    public class FixtureViewModel
    {
        public int FixtureId { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }

        public FixtureViewModel()
        {
        }

        public FixtureViewModel(IEnumerable<TeamModel> teams)
        {
            this.Teams = teams.OrderBy(x => x.Name).Select(x => new SelectListItem { Value = x.TeamId.ToString(), Text = x.Name });
        }

        public FixtureViewModel(FixtureModel fixture, IEnumerable<TeamModel> teams)
            : this(teams)
        {
            this.FixtureId = fixture.FixtureId;
            this.Date = fixture.Date;
            this.HomeTeamId = fixture.HomeTeamId;
            this.AwayTeamId = fixture.AwayTeamId;
            this.HomeTeamGoals = fixture.HomeTeamGoals;
            this.AwayTeamGoals = fixture.AwayTeamGoals;
        }
    }
}