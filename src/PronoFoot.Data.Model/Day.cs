using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public class Day
    {
        public int DayId { get; set; }
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Fixture> Fixtures { get; set; }
    }
}
