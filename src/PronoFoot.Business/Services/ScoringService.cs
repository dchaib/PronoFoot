using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;

namespace PronoFoot.Business.Services
{
    public class ScoringService : IScoringService
    {
        public double GetScore(int homeGoals, int awayGoals, int forecastHomeGoals, int forecastAwayGoals)
        {
            if (homeGoals == awayGoals)
            {
                if (forecastHomeGoals != forecastAwayGoals)
                    return 0;

                if (homeGoals == forecastHomeGoals)
                    return 3;

                if (Math.Abs(forecastHomeGoals - homeGoals) == 1)
                    return 2;

                return 1;
            }
            else
            {
                if ((homeGoals - awayGoals) * (forecastHomeGoals - forecastAwayGoals) <= 0)
                    return 0;

                if (homeGoals == forecastHomeGoals && awayGoals == forecastAwayGoals)
                    return 3;

                if ((Math.Abs(homeGoals - forecastHomeGoals) == 1 && (awayGoals == forecastAwayGoals))
                    || ((homeGoals == forecastHomeGoals) && Math.Abs(awayGoals - forecastAwayGoals) == 1))
                    return 2;

                if ((homeGoals - awayGoals) == (forecastHomeGoals - forecastAwayGoals) && Math.Abs(homeGoals - forecastHomeGoals) == 1)
                    return 1.5;

                return 1;
            }
        }
    }
}
