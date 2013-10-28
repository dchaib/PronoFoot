using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;
using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Services
{
    public class TeamStandingService : ITeamStandingService
    {
        private readonly IFixtureRepository fixtureRepository;
        private readonly ITeamRepository teamRepository;

        public TeamStandingService(IFixtureRepository fixtureRepository, ITeamRepository teamRepository)
        {
            this.fixtureRepository = fixtureRepository;
            this.teamRepository = teamRepository;
        }

        public IEnumerable<TeamStanding> GetTeamStandings(int editionId)
        {
            var fixtures = fixtureRepository.GetFixturesWithResultForEdition(editionId);
            var teams = teamRepository.GetTeamsForEdition(editionId);
            var homeStats = GetHomeStatistics(fixtures);
            var awayStats = GetAwayStatistics(fixtures);

            var teamStats = from team in teams
                            join homeStat in homeStats on team.TeamId equals homeStat.TeamId
                            join awayStat in awayStats on team.TeamId equals awayStat.TeamId
                            select new TeamStanding
                            {
                                TeamId = team.TeamId,
                                TeamName = team.Name,
                                HomeStatistics = homeStat,
                                AwayStatistics = awayStat,
                                OverallStatistics = new TeamStatistics
                                {
                                    TeamId = team.TeamId,
                                    Matches = homeStat.Matches + awayStat.Matches,
                                    Wins = homeStat.Wins + awayStat.Wins,
                                    Draws = homeStat.Draws + awayStat.Draws,
                                    Losses = homeStat.Losses + awayStat.Losses,
                                    GoalsFor = homeStat.GoalsFor + awayStat.GoalsFor,
                                    GoalsAgainst = homeStat.GoalsAgainst + awayStat.GoalsAgainst
                                }
                            };

            int overallRank = 1;
            var q = from teamStat in teamStats
                    group teamStat by new { teamStat.OverallStatistics.Points, teamStat.OverallStatistics.GoalDifference, teamStat.OverallStatistics.GoalsFor } into rankGroup
                    orderby rankGroup.Key.Points descending, rankGroup.Key.GoalDifference descending, rankGroup.Key.GoalsFor descending
                    let currentRank = overallRank++
                    from team in rankGroup
                    let dummy = team.Position = currentRank
                    select team;

            return q.ToList();
        }

        public static IEnumerable<TeamStatistics> GetHomeStatistics(IEnumerable<Fixture> fixtures)
        {
            var q = from f in fixtures
                    let gd = f.HomeTeamGoals - f.AwayTeamGoals
                    group f by f.HomeTeamId into g
                    select new TeamStatistics
                    {
                        TeamId = g.Key,
                        Matches = g.Count(),
                        Wins = g.Count(x => x.HomeTeamGoals > x.AwayTeamGoals),
                        Draws = g.Count(x => x.HomeTeamGoals == x.AwayTeamGoals),
                        Losses = g.Count(x => x.HomeTeamGoals < x.AwayTeamGoals),
                        GoalsFor = g.Sum(x => x.HomeTeamGoals.Value),
                        GoalsAgainst = g.Sum(x => x.AwayTeamGoals.Value)
                    };
            return q.AsEnumerable();
        }

        private static IEnumerable<TeamStatistics> GetAwayStatistics(IEnumerable<Fixture> fixtures)
        {
            var q = from f in fixtures
                    group f by f.AwayTeamId into g
                    select new TeamStatistics
                    {
                        TeamId = g.Key,
                        Matches = g.Count(),
                        Wins = g.Count(x => x.AwayTeamGoals > x.HomeTeamGoals),
                        Draws = g.Count(x => x.AwayTeamGoals == x.HomeTeamGoals),
                        Losses = g.Count(x => x.AwayTeamGoals < x.HomeTeamGoals),
                        GoalsFor = g.Sum(x => x.AwayTeamGoals.Value),
                        GoalsAgainst = g.Sum(x => x.HomeTeamGoals.Value)
                    };
            return q.AsEnumerable();
        }
    }
}
