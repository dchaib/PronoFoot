using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PronoFoot.Business.Services;
using PronoFoot.Data;
using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Tests.Services
{
    [TestClass]
    public class TeamStandingServiceTests
    {
        [TestMethod]
        public void HomeStatisticsTest()
        {
            IEnumerable<Fixture> fixtures = new List<Fixture>()
            {
                new Fixture{ HomeTeamId=1, AwayTeamId=2, HomeTeamGoals=0, AwayTeamGoals=1},
                new Fixture{ HomeTeamId=3, AwayTeamId=4, HomeTeamGoals=1, AwayTeamGoals=1},
                new Fixture{ HomeTeamId=2, AwayTeamId=3, HomeTeamGoals=2, AwayTeamGoals=0},
                new Fixture{ HomeTeamId=4, AwayTeamId=1, HomeTeamGoals=0, AwayTeamGoals=0},
                new Fixture{ HomeTeamId=3, AwayTeamId=1, HomeTeamGoals=3, AwayTeamGoals=2},
                new Fixture{ HomeTeamId=4, AwayTeamId=2, HomeTeamGoals=1, AwayTeamGoals=0},
            };

            var homeStats = TeamStandingService.GetHomeStatistics(fixtures);

            Assert.AreEqual(4, homeStats.Count());

            var homeStatsTeam3 = homeStats.SingleOrDefault(x => x.TeamId == 3);
            Assert.IsNotNull(homeStatsTeam3);
            Assert.AreEqual(2, homeStatsTeam3.Matches);
            Assert.AreEqual(1, homeStatsTeam3.Wins);
            Assert.AreEqual(1, homeStatsTeam3.Draws);
            Assert.AreEqual(0, homeStatsTeam3.Losses);
            Assert.AreEqual(4, homeStatsTeam3.GoalsFor);
            Assert.AreEqual(3, homeStatsTeam3.GoalsAgainst);
        }


        [TestMethod]
        public void CheckStandingsOrder()
        {
            IEnumerable<Fixture> fixtures = new List<Fixture>()
            {
                new Fixture{ HomeTeamId=1, AwayTeamId=2, HomeTeamGoals=0, AwayTeamGoals=1},
                new Fixture{ HomeTeamId=3, AwayTeamId=4, HomeTeamGoals=1, AwayTeamGoals=1},
                new Fixture{ HomeTeamId=2, AwayTeamId=3, HomeTeamGoals=2, AwayTeamGoals=0},
                new Fixture{ HomeTeamId=4, AwayTeamId=1, HomeTeamGoals=0, AwayTeamGoals=0},
                new Fixture{ HomeTeamId=3, AwayTeamId=1, HomeTeamGoals=3, AwayTeamGoals=2},
                new Fixture{ HomeTeamId=4, AwayTeamId=2, HomeTeamGoals=1, AwayTeamGoals=0},
            };
            IEnumerable<Team> teams = new List<Team>()
            {
                new Team { TeamId = 1, Name = "Team 1" },
                new Team { TeamId = 2, Name = "Team 2" },
                new Team { TeamId = 3, Name = "Team 3" },
                new Team { TeamId = 4, Name = "Team 4" }
            };

            var fixtureRepository = new Mock<IFixtureRepository>();
            fixtureRepository.Setup(x => x.GetFixturesForEdition(It.IsAny<int>())).Returns(() => fixtures);
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(x => x.GetTeamsForEdition(It.IsAny<int>())).Returns(() => teams);
            var service = new TeamStandingService(fixtureRepository.Object, teamRepository.Object);

            var standings = service.GetTeamStandings(0);

            Assert.AreEqual(4, standings.Count());

            foreach (var item in standings)
            {
                switch (item.TeamId)
                {
                    case 1:
                        Assert.AreEqual(4, item.Position);
                        break;
                    case 2:
                        Assert.AreEqual(1, item.Position);
                        break;
                    case 3:
                        Assert.AreEqual(3, item.Position);
                        break;
                    case 4:
                        Assert.AreEqual(2, item.Position);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
