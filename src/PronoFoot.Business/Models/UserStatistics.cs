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
            NumberOfExactForecasts = forecasts.Count(f => f.Rating.Value == Rating.Exact);
            NumberOfCloseForecasts = forecasts.Count(f => f.Rating.Value == Rating.Close);
            NumberOfForecastsWithExactDifference = forecasts.Count(f => f.Rating.Value == Rating.CorrectDifference);
            NumberOfCorrect1N2Forecasts = forecasts.Count(f => f.Rating.Value == Rating.CorrectOutcome);
            NumberOfWrongForecasts = forecasts.Count(f => f.Rating.Value == Rating.Wrong);
        }
    }
}
