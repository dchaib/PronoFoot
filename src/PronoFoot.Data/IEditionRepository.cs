using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Data
{
    public interface IEditionRepository
    {
        void AddTeamToEdition(int editionId, Team team);
        void RemoveTeamFromEdition(int editionId, int teamId);

        IEnumerable<Edition> GetEditions(int competitionId);

        Edition GetEdition(int id);
    }
}
