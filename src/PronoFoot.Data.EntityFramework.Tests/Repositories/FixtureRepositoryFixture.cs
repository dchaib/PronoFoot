using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PronoFoot.Data.Model;
using PronoFoot.Data.EntityFramework.Repositories;

namespace PronoFoot.Data.EntityFramework.Tests.Repositories
{
    [TestClass]
    public class FixtureRepositoryFixture
    {
        private Competition testCompetition;
        private Day testDay;
        private Team testTeam1;
        private Team testTeam2;

        [TestInitialize]
        public void Initialize()
        {
            DatabaseTestUtility.DropCreateDatabase();

            var uow = this.GetUnitOfWork();

            var competitionRepository = new CompetitionRepository(uow);
            this.testCompetition = new Competition() { Name = "TestCompetition" };
            competitionRepository.Create(this.testCompetition);

            var dayRepository = new DayRepository(uow);
            this.testDay = new Day() { Name = "TestDay", Date = DateTime.Today };
            dayRepository.Create(this.testCompetition.CompetitionId, this.testDay);

            var teamRepository = new TeamRepository(uow);
            this.testTeam1 = new Team() { Name = "TestTeam1" };
            teamRepository.Create(this.testTeam1);
            this.testTeam2 = new Team() { Name = "TestTeam2" };
            teamRepository.Create(this.testTeam2);
        }

        [TestMethod]
        public void WhenAddingFixture_ThenPersistsFixture()
        {
            var repository = new FixtureRepository(this.GetUnitOfWork());

            var fixtureDate = DateTime.Now;
            var newFixture = new Fixture()
            {
                Date = fixtureDate,
                HomeTeamId = this.testTeam1.TeamId,
                AwayTeamId = this.testTeam2.TeamId
            };

            repository.Create(this.testDay.DayId, newFixture);

            var actualFixtures = new PronoFootDbContext().Fixtures.ToList();

            Assert.AreEqual(1, actualFixtures.Count());
            Assert.AreEqual(fixtureDate.ToString(), actualFixtures[0].Date.ToString());
        }

        [TestMethod]
        public void WhenAddingFixture_ThenFixtureIdUpdates()
        {
            var repository = new FixtureRepository(this.GetUnitOfWork());

            var newFixture = new Fixture()
            {
                Date = DateTime.Now,
                HomeTeamId = this.testTeam1.TeamId,
                AwayTeamId = this.testTeam2.TeamId
            };

            repository.Create(this.testDay.DayId, newFixture);

            var actualFixtures = new PronoFootDbContext().Fixtures.ToList();

            Assert.AreEqual(1, actualFixtures.Count());
            Assert.AreEqual(1, actualFixtures[0].FixtureId);
        }

        [TestMethod]
        public void WhenAddingTeam_ThenReturnsPopulatedTeam()
        {

            var repository = new FixtureRepository(this.GetUnitOfWork());

            var fixtureDate = DateTime.Now;
            var newFixture = new Fixture()
            {
                Date = fixtureDate,
                HomeTeamId = this.testTeam1.TeamId,
                AwayTeamId = this.testTeam2.TeamId
            };

            repository.Create(this.testDay.DayId, newFixture);

            Assert.IsNotNull(newFixture);
            Assert.AreEqual(1, newFixture.FixtureId);
            Assert.AreEqual(this.testDay.DayId, newFixture.DayId);
            Assert.AreEqual(fixtureDate.ToString(), newFixture.Date.ToString());
            Assert.AreEqual(this.testTeam1.TeamId, newFixture.HomeTeamId);
            Assert.AreEqual(this.testTeam2.TeamId, newFixture.AwayTeamId);
        }

        private IUnitOfWork GetUnitOfWork()
        {
            return new PronoFootDbContext();
        }
    }
}
