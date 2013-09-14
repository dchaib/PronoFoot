using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Data.Model
{
    public class Edition
    {
        public int EditionId { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Day> Days { get; set; }
    }
}
