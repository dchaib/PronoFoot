using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Models
{
    public class EditionModel
    {
        public int EditionId { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }

        public EditionModel()
        {
        }

        public EditionModel(Edition edition)
        {
            this.EditionId = edition.EditionId;
            this.CompetitionId = edition.CompetitionId;
            this.Name = edition.Name;            
        }
    }
}
