using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PronoFoot.Data.Model;

namespace PronoFoot.ViewModels
{
    public class HomeViewModel
    {
        public Competition Competition { get; set; }
        public IEnumerable<DayViewModel> Days { get; set; }
        public IEnumerable<UserScoreViewModel> Scores { get; set; }
    }
}