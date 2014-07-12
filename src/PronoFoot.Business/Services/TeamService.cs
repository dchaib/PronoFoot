using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;

namespace PronoFoot.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository teamRepository;
        private readonly IFixtureRepository fixtureRepository;

        public TeamService(ITeamRepository teamRepository, IFixtureRepository fixtureRepository)
        {
            this.teamRepository = teamRepository;
            this.fixtureRepository = fixtureRepository;
        }

        public IEnumerable<TeamModel> GetTeamsForEdition(int editionId)
        {
            var teams = teamRepository.GetTeamsForEdition(editionId);

            var teamModels = teams.Select(x => new TeamModel(x));

            return teamModels.ToList();
        }

        public IDictionary<int, IEnumerable<FixtureModel>> GetTeamLastestFixtures(int editionId)
        {
            var fixtures = fixtureRepository.GetFixturesWithResultForEdition(editionId).ToList();
            var q = from team in teamRepository.GetTeamsForEdition(editionId)
                    join homeFixture in fixtures on team.TeamId equals homeFixture.HomeTeamId into homeFixtures
                    join awayFixture in fixtures on team.TeamId equals awayFixture.AwayTeamId into awayFixtures
                    select new { Team = team, LatestFixtures = homeFixtures.Union(awayFixtures).OrderByDescending(x => x.Date).Take(5).OrderBy(x => x.Date) };
            return q.ToDictionary(x => x.Team.TeamId, y => y.LatestFixtures.Select(x => new FixtureModel(x)));
        }
    }
}
