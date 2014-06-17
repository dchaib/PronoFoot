using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.Models.Competition
{
    public class CompetitionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasTeamClassification { get; set; }
    }
}