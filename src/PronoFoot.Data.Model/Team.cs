using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }

        public ICollection<Competition> Competitions { get; set; }
    }
}
