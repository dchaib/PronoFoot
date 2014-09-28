using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class UserScoreViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public double Score { get; set; }
        public int NumberOfExactForecasts { get; set; }
        public int NumberOfCloseForecasts { get; set; }
        public int NumberOfForecastsWithExactDifference { get; set; }
        public int NumberOfCorrect1N2Forecasts { get; set; }
        public int NumberOfWrongForecasts { get; set; }
        public double PercentageOfScoringForecasts { get; set; }
    }
}