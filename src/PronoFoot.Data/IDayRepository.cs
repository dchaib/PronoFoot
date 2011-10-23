using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface IDayRepository
    {
        void Create(int competitionId, Day day);

        Day GetDay(int dayId);
        IEnumerable<Day> GetDays(int competitionId);

        void Update(Day day);
    }
}
