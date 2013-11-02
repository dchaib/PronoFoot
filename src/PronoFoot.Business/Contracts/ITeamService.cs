using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface ITeamService
    {
        IEnumerable<TeamModel> GetTeamsForEdition(int competitionId);
        IDictionary<int, IEnumerable<FixtureModel>> GetTeamLastestFixtures(int editionId);
    }
}
