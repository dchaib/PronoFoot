using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Business.Models;
using PronoFoot.Data.Model;

namespace PronoFoot.ViewModels
{
    public class DayDetailsViewModel
    {
        public DayModel Day { get; set; }
        public IList<FixtureModel> Fixtures { get; set; }
        public IList<TeamModel> Teams { get; set; }
        public IEnumerable<ForecastModel> Forecasts { get; set; }
        public IList<User> Users { get; set; }
        public int CurrentUserId { get; set; }
    }
}