using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.Models.Competition
{
    public class CompetitionListModel
    {
        public IList<CompetitionModel> Competitions { get; set; }
    }
}