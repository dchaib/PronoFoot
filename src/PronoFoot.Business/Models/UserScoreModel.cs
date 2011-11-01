using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Business.Models
{
    public class UserScoreModel
    {
        public int UserId { get; set; }
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
    }
}
