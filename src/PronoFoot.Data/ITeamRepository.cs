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
        IEnumerable<Team> GetTeamsForEdition(int editionId);

        void Update(Team team);
    }
}
