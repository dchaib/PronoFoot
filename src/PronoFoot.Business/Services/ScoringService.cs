using PronoFoot.Business.Contracts;
using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Business.Services
{
    public class ScoringService : IScoringService
    {
        public Rating GetRating(int homeGoals, int awayGoals, int forecastHomeGoals, int forecastAwayGoals)
        {
            if (homeGoals == awayGoals)
            {
                if (forecastHomeGoals != forecastAwayGoals)
                    return Rating.Wrong;

                if (homeGoals == forecastHomeGoals)
                    return Rating.Exact;

                if (Math.Abs(forecastHomeGoals - homeGoals) == 1)
                    return Rating.Close;

                return Rating.CorrectOutcome;
            }
            else
            {
                if ((homeGoals - awayGoals) * (forecastHomeGoals - forecastAwayGoals) <= 0)
                    return Rating.Wrong;

                if (homeGoals == forecastHomeGoals && awayGoals == forecastAwayGoals)
                    return Rating.Exact;

                if ((Math.Abs(homeGoals - forecastHomeGoals) == 1 && (awayGoals == forecastAwayGoals))
                    || ((homeGoals == forecastHomeGoals) && Math.Abs(awayGoals - forecastAwayGoals) == 1))
                    return Rating.Close;

                if ((homeGoals - awayGoals) == (forecastHomeGoals - forecastAwayGoals) && Math.Abs(homeGoals - forecastHomeGoals) == 1)
                    return Rating.CorrectDifference;

                return Rating.CorrectOutcome;
            }
        }

        public double GetScore(Rating rating)
        {
            switch (rating)
            {
                case Rating.Wrong:
                    return 0;
                case Rating.CorrectOutcome:
                    return 1;
                case Rating.CorrectDifference:
                    return 1.5;
                case Rating.Close:
                    return 2;
                case Rating.Exact:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
