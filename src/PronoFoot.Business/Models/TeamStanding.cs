using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Models
{

    public class TeamStanding
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Position { get; set; }
        public TeamStatistics HomeStatistics { get; set; }
        public TeamStatistics AwayStatistics { get; set; }
        public TeamStatistics OverallStatistics { get; set; }
    }
}
