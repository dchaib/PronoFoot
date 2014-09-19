using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class EditionViewModel
    {
        public int EditionId { get; set; }
        public string Name { get; set; }
        public string CompetitionName { get; set; }
        public bool HasTeamClassification { get; set; }
    }
}