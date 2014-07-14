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
        public FixtureModel Fixture { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }

        public ForecastViewModel()
        {
        }

        public class Team
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int? Standing { get; set; }
            public IEnumerable<TeamResult> Fixtures { get; set; }

            public Team(TeamModel team, IEnumerable<FixtureModel> fixtures, int? standing)
            {
                Id = team.TeamId;
                Name = team.Name;
                Fixtures = fixtures.Any() ? fixtures.Select(x => new TeamResult(x, team.TeamId)).ToList() : Enumerable.Empty<TeamResult>();
                Standing = standing;
            }
        }

        public class TeamResult
        {
            public DateTime Date { get; set; }
            public FixtureLocation Location { get; set; }
            public FixtureResult Result { get; set; }
            public string Score { get; set; }

            public TeamResult(FixtureModel fixture, int teamId)
            {
                Date = fixture.Date;
                Location = fixture.HomeTeamId == teamId ? PronoFoot.ViewModels.ForecastViewModel.FixtureLocation.Home : ForecastViewModel.FixtureLocation.Away;
                Result = GetResult(fixture, teamId);
                Score = fixture.HomeTeamGoals.Value + "-" + fixture.AwayTeamGoals.Value;
            }

            private static FixtureResult GetResult(FixtureModel fixture, int teamId)
            {
                if (teamId == fixture.HomeTeamId)
                {
                    if (fixture.HomeTeamGoals.Value > fixture.AwayTeamGoals.Value)
                        return FixtureResult.Won;
                    else if (fixture.HomeTeamGoals.Value == fixture.AwayTeamGoals.Value)
                        return FixtureResult.Drawn;
                    else
                        return FixtureResult.Lost;
                }
                else
                {
                    if (fixture.HomeTeamGoals.Value > fixture.AwayTeamGoals.Value)
                        return FixtureResult.Lost;
                    else if (fixture.HomeTeamGoals.Value == fixture.AwayTeamGoals.Value)
                        return FixtureResult.Drawn;
                    else
                        return FixtureResult.Won;
                }
            }
        }

        public enum FixtureLocation
        {
            Home,
            Away
        }

        public enum FixtureResult
        {
            Won,
            Drawn,
            Lost
        }
    }
}