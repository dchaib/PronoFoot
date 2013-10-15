using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface IDayRepository
    {
        int Create(int competitionId, Day day);

        Day GetDay(int dayId);
        IEnumerable<Day> GetDays(int editionId);
        IEnumerable<Day> GetDays(int[] editionIds);

        void Update(Day day);
    }
}
