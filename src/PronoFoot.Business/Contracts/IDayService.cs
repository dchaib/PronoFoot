using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface IDayService
    {
        DayModel GetDay(int id);
        IEnumerable<DayModel> GetDaysForCompetition(int competitionId);
        int Create(DayModel day, IEnumerable<FixtureModel> fixtures);
        void Update(DayModel day, IEnumerable<FixtureModel> fixtures);
    }
}
