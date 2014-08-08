using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Models
{

    public class TeamStanding
    {
        public int TeamId { get; private set; }
        public string TeamName { get; private set; }
        public int Position { get; set; }
        public TeamStatistics HomeStatistics { get; private set; }
        public TeamStatistics AwayStatistics { get; private set; }
        public TeamStatistics OverallStatistics { get; private set; }

        public TeamStanding(int teamId, string teamName, TeamStatistics homeStatistics, TeamStatistics awayStatistics)
        {
            TeamId = teamId;
            TeamName = teamName;
            HomeStatistics = homeStatistics;
            AwayStatistics = awayStatistics;
            UpdateOverallStatistics();
        }
        
        private void UpdateOverallStatistics()
        {
            var overallStatistics = new TeamStatistics { TeamId = TeamId };
            if (HomeStatistics != null)
            {
                overallStatistics.Matches += HomeStatistics.Matches;
                overallStatistics.Wins += HomeStatistics.Wins;
                overallStatistics.Draws += HomeStatistics.Draws;
                overallStatistics.Losses += HomeStatistics.Losses;
                overallStatistics.GoalsFor += HomeStatistics.GoalsFor;
                overallStatistics.GoalsAgainst += HomeStatistics.GoalsAgainst;
            }
            if (AwayStatistics != null)
            {
                overallStatistics.Matches += AwayStatistics.Matches;
                overallStatistics.Wins += AwayStatistics.Wins;
                overallStatistics.Draws += AwayStatistics.Draws;
                overallStatistics.Losses += AwayStatistics.Losses;
                overallStatistics.GoalsFor += AwayStatistics.GoalsFor;
                overallStatistics.GoalsAgainst += AwayStatistics.GoalsAgainst;
            }
            OverallStatistics = overallStatistics;
        }
    }
}
