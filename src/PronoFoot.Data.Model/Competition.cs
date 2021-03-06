﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public class Competition
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public bool HasTeamClassification { get; set; }

        public virtual ICollection<Edition> Editions { get; set; }
    }
}
