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

        public TeamService(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public IEnumerable<TeamModel> GetTeamsForCompetition(int competitionId)
        {
            var teams = teamRepository.GetTeamsForCompetition(competitionId);

            var teamModels = teams.Select(x => new TeamModel(x));

            return teamModels.ToList();
        }
    }
}
