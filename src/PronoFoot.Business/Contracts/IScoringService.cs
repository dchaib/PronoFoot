using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Business.Contracts
{
    public interface IScoringService
    {
        double GetScore(int homeGoals, int awayGoals, int forecastHomeGoals, int forecastAwayGoals);
    }
}
