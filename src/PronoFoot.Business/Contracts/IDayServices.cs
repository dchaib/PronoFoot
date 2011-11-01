using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface IDayServices
    {
        DayModel GetDay(int id);
        void Update(DayModel day, IEnumerable<FixtureModel> fixtures);
    }
}
