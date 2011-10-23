using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Business.Models;

namespace PronoFoot.ViewModels
{
    public class DayDetailsViewModel
    {
        public DayModel Day { get; set; }
        public IList<FixtureModel> Fixtures { get; set; }
        public IList<TeamModel> Teams { get; set; }
        public IList<ForecastModel> Forecasts { get; set; }
    }
}