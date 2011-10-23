using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface ITeamRepository
    {
        void Create(Team team);

        Team GetTeam(int teamId);
        IEnumerable<Team> GetTeams();
        IEnumerable<Team> GetTeamsForCompetition(int competitionId);

        void Update(Team team);
    }
}
