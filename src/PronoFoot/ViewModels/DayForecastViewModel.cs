using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Business.Models;

namespace PronoFoot.ViewModels
{
    public class DayForecastViewModel
    {
        public DayModel Day { get; set; }
        public IList<TeamModel> Teams { get; set; }
        public IList<ForecastViewModel> Forecasts { get; set; }
        public IDictionary<int, IEnumerable<FixtureModel>> LatestFixtures { get; set; }
    }
}