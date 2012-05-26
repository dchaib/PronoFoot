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
        IList<DayModel> GetDaysForCompetition(int competitionId);
        IList<DayModel> GetDaysForCompetitions(int[] competitionIds);
        DayModel GetPreviousDay(int competitionId);
        DayModel GetNextDay(int competitionId);
        int Create(DayModel day, IEnumerable<FixtureModel> fixtures);
        void Update(DayModel day, IEnumerable<FixtureModel> fixtures);
    }
}
