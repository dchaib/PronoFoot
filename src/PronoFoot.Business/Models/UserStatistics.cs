using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class UserStatistics
    {
        public int Key { get; set; }
        public double Score { get; set; }
        public int NumberOfExactForecasts { get; set; }
        public int NumberOfCloseForecasts { get; set; }
        public int NumberOfForecastsWithExactDifference { get; set; }
        public int NumberOfCorrect1N2Forecasts { get; set; }
        public int NumberOfWrongForecasts { get; set; }

        public int NumberOfScoringForecasts
        {
            get
            {
                return NumberOfExactForecasts + NumberOfCloseForecasts + NumberOfForecastsWithExactDifference + NumberOfCorrect1N2Forecasts;
            }
        }
        public int NumberOfForecasts
        {
            get
            {
                return NumberOfScoringForecasts + NumberOfWrongForecasts;
            }
        }

        public double PercentageOfScoringForecasts
        {
            get
            {
                return NumberOfForecasts > 0 ? (double)NumberOfScoringForecasts / NumberOfForecasts : 0;
            }
        }

        public UserStatistics(int key, IEnumerable<Forecast> forecasts)
        {
            Key = key;
            Score = forecasts.Sum(f => f.Score.Value);
            NumberOfExactForecasts = forecasts.Count(f => f.Score.Value == 3);
            NumberOfCloseForecasts = forecasts.Count(f => f.Score.Value == 2);
            NumberOfForecastsWithExactDifference = forecasts.Count(f => f.Score.Value == 1.5);
            NumberOfCorrect1N2Forecasts = forecasts.Count(f => f.Score.Value == 1);
            NumberOfWrongForecasts = forecasts.Count(f => f.Score.Value == 0);
        }
    }
}
