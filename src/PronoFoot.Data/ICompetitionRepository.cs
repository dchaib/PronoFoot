using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface ICompetitionRepository
    {
        int Create(Competition competition);

        Competition GetCompetition(int competitionId);
        IEnumerable<Competition> GetCompetitions();

        void Update(Competition competition);
    }
}
