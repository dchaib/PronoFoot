﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class HomeViewModel
    {
        public CompetitionViewModel Competition { get; set; }
        public IEnumerable<DayViewModel> Days { get; set; }
        public IEnumerable<UserScoreViewModel> Scores { get; set; }
    }
}