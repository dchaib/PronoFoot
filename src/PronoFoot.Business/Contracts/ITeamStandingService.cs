using PronoFoot.Business.Models;
using System;
using System.Collections.Generic;

namespace PronoFoot.Business.Contracts
{
    public interface ITeamStandingService
    {
       IEnumerable<TeamStanding> GetTeamStandings(int editionId);
    }
}
