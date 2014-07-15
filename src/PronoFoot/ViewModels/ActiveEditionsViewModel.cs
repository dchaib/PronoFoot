using PronoFoot.Models.Edition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class ActiveEditionsViewModel
    {
        public IEnumerable<EditionOverviewModel> CurrentEditions { get; set; }
        public IEnumerable<EditionOverviewModel> UpcomingEditions { get; set; }
        public IEnumerable<EditionOverviewModel> RecentEditions { get; set; }
    }
}