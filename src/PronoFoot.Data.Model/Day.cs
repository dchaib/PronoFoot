using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public class Day
    {
        public int DayId { get; set; }
        public int EditionId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Coefficient { get; set; }

        public virtual Edition Edition { get; set; }
        public virtual ICollection<Fixture> Fixtures { get; set; }
    }
}
