using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class HomeRulesExampleViewModel
    {
        public int ScoreHomeGoals { get; set; }
        public int ScoreAwayGoals { get; set; }
        public int ForecastHomeGoals { get; set; }
        public int ForecastAwayGoals { get; set; }
        public double Score { get; set; }
    }
}