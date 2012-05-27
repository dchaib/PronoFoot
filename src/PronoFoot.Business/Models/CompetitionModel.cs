using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class CompetitionModel
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }

        public CompetitionModel()
        {
        }

        public CompetitionModel(Competition competition)
        {
            this.CompetitionId = competition.CompetitionId;
            this.Name = competition.Name;            
        }
    }
}
