using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public CompetitionViewModel Competition { get; set; }
        public IEnumerable<DayViewModel> PreviousDays { get; set; }
        public IEnumerable<DayViewModel> NextDays { get; set; }
        public IEnumerable<UserScoreViewModel> Scores { get; set; }
    }
}