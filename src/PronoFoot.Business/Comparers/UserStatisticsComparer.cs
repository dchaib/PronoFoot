using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Comparers
{
    public class UserStatisticsComparer : IComparer<UserStatistics>
    {
        public int Compare(UserStatistics x, UserStatistics y)
        {
            int result = x.Score.CompareTo(y.Score);
            if (result == 0)
            {
                result = x.NumberOfExactForecasts.CompareTo(y.NumberOfExactForecasts);
                if (result == 0)
                {
                    result = x.NumberOfCloseForecasts.CompareTo(y.NumberOfCloseForecasts);
                    if (result == 0)
                    {
                        result = x.NumberOfForecastsWithExactDifference.CompareTo(y.NumberOfForecastsWithExactDifference);
                        if (result == 0)
                        {
                            result = x.NumberOfCorrect1N2Forecasts.CompareTo(y.NumberOfCorrect1N2Forecasts);
                            if (result == 0)
                            {
                                result = x.PercentageOfScoringForecasts.CompareTo(y.PercentageOfScoringForecasts);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
