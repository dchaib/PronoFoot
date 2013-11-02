using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Models
{
    public class TeamStatistics
    {
        public int TeamId { get; set; }
        public int Matches { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        public int Points
        {
            get { return 3 * Wins + Draws; }
        }

        public int GoalDifference
        {
            get { return GoalsFor - GoalsAgainst; }
        }
    }
}
