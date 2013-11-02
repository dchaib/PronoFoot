using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface ICompetitionService
    {
        IList<CompetitionModel> GetCompetitions();
        CompetitionModel GetCompetition(int id);

        int Create(CompetitionModel competition);
        void Update(CompetitionModel competition);
    }
}
