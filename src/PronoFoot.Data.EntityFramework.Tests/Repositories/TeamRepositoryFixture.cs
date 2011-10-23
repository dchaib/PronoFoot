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
    public class TeamRepositoryFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseTestUtility.DropCreateDatabase();
        }

        [TestMethod]
        public void WhenAddingTeam_ThenTeamPersists()
        {
            var repository = new TeamRepository(this.GetUnitOfWork());

            string teamName = "TheTeamName";
            var newTeam = new Team()
            {
                Name = teamName
            };

            repository.Create(newTeam);

            var actualTeams = new PronoFootDbContext().Teams.ToList();

            Assert.AreEqual(1, actualTeams.Count());
            Assert.AreEqual(teamName, actualTeams[0].Name);
        }

        [TestMethod]
        public void WhenAddingTeam_ThenTeamIdUpdates()
        {
            var repository = new TeamRepository(this.GetUnitOfWork());

            string teamName = "TheTeamName";
            var newTeam = new Team()
            {
                Name = teamName
            };

            repository.Create(newTeam);

            var actualTeams = new PronoFootDbContext().Teams.ToList();

            Assert.AreEqual(1, actualTeams.Count());
            Assert.AreEqual(1, actualTeams[0].TeamId);
        }

        [TestMethod]
        public void WhenAddingTeam_ThenReturnsPopulatedTeam()
        {
            var repository = new TeamRepository(this.GetUnitOfWork());

            string teamName = "TheTeamName";
            var newTeam = new Team()
            {
                Name = teamName
            };

            repository.Create(newTeam);

            Assert.IsNotNull(newTeam);
            Assert.AreEqual(1, newTeam.TeamId);
            Assert.AreEqual(teamName, newTeam.Name);
        }

        [TestMethod]
        public void WhenRequestingTeam_ThenReturnsTeam()
        {
            var team = new Team { Name = "TheTeamName" };
            var expectedId = 1;

            using (var context = new PronoFootDbContext())
            {
                context.Teams.Add(team);
                context.SaveChanges();
            }

            var repository = new TeamRepository(this.GetUnitOfWork());

            Team retrievedTeam = repository.GetTeam(expectedId);

            Assert.IsNotNull(retrievedTeam);
            Assert.AreEqual(expectedId, retrievedTeam.TeamId);
            Assert.AreEqual(team.Name, retrievedTeam.Name);
        }

        [TestMethod]
        public void WhenGettingAllTeamsFromEmptyDatabase_ThenReturnsEmptyCollection()
        {
            var repository = new TeamRepository(this.GetUnitOfWork());
            IEnumerable<Team> actual = repository.GetTeams();

            Assert.IsNotNull(actual);
            var actualList = new List<Team>(actual);
            Assert.AreEqual(0, actualList.Count);
        }

        [TestMethod]
        public void WhenModifyingTeam_ThenPersistsChange()
        {
            var repository = new TeamRepository(this.GetUnitOfWork());

            var team = new Team { Name = "MyTeam" };
            repository.Create(team);

            // I use a new context and repository to verify the data was stored
            var repositoryForUpdate = new TeamRepository(this.GetUnitOfWork());

            var retrievedTeam = repositoryForUpdate.GetTeams().First();

            var updatedName = "Updated Team Name";
            retrievedTeam.Name = updatedName;
            repositoryForUpdate.Update(retrievedTeam);
            int updatedTeamId = retrievedTeam.TeamId;

            var repositoryForVerify = new TeamRepository(this.GetUnitOfWork());
            var updatedTeam = repositoryForVerify.GetTeam(updatedTeamId);

            Assert.AreEqual(updatedName, updatedTeam.Name);
        }

        private IUnitOfWork GetUnitOfWork()
        {
            return new PronoFootDbContext();
        }
    }
}
