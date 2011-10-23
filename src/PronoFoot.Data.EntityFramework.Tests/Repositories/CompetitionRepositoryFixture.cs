using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PronoFoot.Data.EntityFramework.Repositories;
using PronoFoot.Data.Model;

namespace PronoFoot.Data.EntityFramework.Tests.Repositories
{
    [TestClass]
    public class CompetitionRepositoryFixture
    {
        [TestInitialize]
        public void Initialize()
        {
            DatabaseTestUtility.DropCreateDatabase();
        }

        [TestMethod]
        public void WhenAddingCompetition_ThenCompetitionPersists()
        {
            var competitionRepository = new CompetitionRepository(new PronoFootDbContext());

            string competitionName = "TheCompetitionName";
            var newCompetition = new Competition()
            {
                Name = competitionName
            };

            competitionRepository.Create(newCompetition);

            var actualCompetitions = new PronoFootDbContext().Competitions.ToList();

            Assert.AreEqual(1, actualCompetitions.Count());
            Assert.AreEqual(competitionName, actualCompetitions[0].Name);
        }

        [TestMethod]
        public void WhenAddingCompetition_ThenCompetitionIdUpdates()
        {
            var competitionRepository = new CompetitionRepository(new PronoFootDbContext());

            string competitionName = "TheCompetitionName";
            var newCompetition = new Competition()
            {
                Name = competitionName
            };

            competitionRepository.Create(newCompetition);

            List<Competition> actualCompetitions = new PronoFootDbContext().Competitions.ToList();

            Assert.AreEqual(1, actualCompetitions.Count());
            Assert.AreEqual(1, actualCompetitions[0].CompetitionId);
        }

        [TestMethod]
        public void WhenAddingCompetition_ThenReturnsPopulatedCompetition()
        {
            var competitionRepository = new CompetitionRepository(new PronoFootDbContext());

            string competitionName = "TheCompetitionName";
            var newCompetition = new Competition()
            {
                Name = competitionName
            };

            competitionRepository.Create(newCompetition);

            Assert.IsNotNull(newCompetition);
            Assert.AreEqual(1, newCompetition.CompetitionId);
            Assert.AreEqual(competitionName, newCompetition.Name);
        }

        [TestMethod]
        public void WhenRequestingCompetition_ThenReturnsCompetition()
        {
            var competition = new Competition { Name = "TheCompetitionName" };
            var expectedId = 1;

            using (var context = new PronoFootDbContext())
            {
                context.Competitions.Add(competition);
                context.SaveChanges();
            }

            var repository = new CompetitionRepository(new PronoFootDbContext());

            Competition retrievedCompetition = repository.GetCompetition(expectedId);

            Assert.IsNotNull(retrievedCompetition);
            Assert.AreEqual(expectedId, retrievedCompetition.CompetitionId);
            Assert.AreEqual(competition.Name, retrievedCompetition.Name);
        }

        [TestMethod]
        public void WhenGettingAllFromEmptyDatabase_ThenReturnsEmptyCollection()
        {
            var repository = new CompetitionRepository(new PronoFootDbContext());
            IEnumerable<Competition> actual = repository.GetCompetitions();

            Assert.IsNotNull(actual);
            var actualList = new List<Competition>(actual);
            Assert.AreEqual(0, actualList.Count);
        }

        [TestMethod]
        public void WhenModifyingCompetition_ThenPersistsChange()
        {
            var repository = new CompetitionRepository(new PronoFootDbContext());

            var competition = new Competition { Name = "MyCompetition" };
            repository.Create(competition);

            // I use a new context and repository to verify the data was stored
            var repositoryForUpdate = new CompetitionRepository(new PronoFootDbContext());

            var retrievedCompetition = repositoryForUpdate.GetCompetitions().First();

            var updatedName = "Updated Competition Name";
            retrievedCompetition.Name = updatedName;
            repositoryForUpdate.Update(retrievedCompetition);
            int updatedCompetitionId = retrievedCompetition.CompetitionId;

            var repositoryForVerify = new CompetitionRepository(new PronoFootDbContext());
            var updatedCompetition = repositoryForVerify.GetCompetition(updatedCompetitionId);

            Assert.AreEqual(updatedName, updatedCompetition.Name);
        }

        [TestMethod]
        public void WhenModifyingCompetitionInSameContext_ThenPersistsChange()
        {
            IUnitOfWork uow = new PronoFootDbContext();
            var repository = new CompetitionRepository(uow);

            var competition = new Competition { Name = "Competition" };
            repository.Create(competition);

            // I use a new context and repository to verify the data was stored
            var repositoryForUpdate = new CompetitionRepository(uow);

            Competition retrievedCompetition = repositoryForUpdate.GetCompetitions().First();

            var updatedName = "Updated Competition Name";
            retrievedCompetition.Name = updatedName;
            repositoryForUpdate.Update(retrievedCompetition);
            int updatedCompetitionId = retrievedCompetition.CompetitionId;

            var repositoryForVerify = new CompetitionRepository(uow);
            Competition updatedCompetition = repositoryForVerify.GetCompetition(updatedCompetitionId);

            Assert.AreEqual(updatedName, updatedCompetition.Name);
        }

        [TestMethod]
        public void WhenAddingTeam_ThenPersistsChange()
        {
            var repository = new CompetitionRepository(new PronoFootDbContext());

            var competition = new Competition { Name = "TheCompetitionName" };
            var team = new Team { Name = "TheTeamName" };

            repository.Create(competition);
            new PronoFootDbContext().Teams.Add(team);

            repository.AddTeamToCompetition(competition.CompetitionId, team);

            var repositoryToVerify = new CompetitionRepository(new PronoFootDbContext());
            Competition retrievedCompetition = repositoryToVerify.GetCompetitions().First();

            Assert.IsNotNull(competition.Teams);
            Assert.AreEqual(1, competition.Teams.Count);
            Assert.AreEqual("TheTeamName", competition.Teams.First().Name);
        }

        [TestMethod]
        public void WhenAddingTeamTwice_ThenDoesNotInsertTwice()
        {
            var repository = new CompetitionRepository(new PronoFootDbContext());

            var competition = new Competition { Name = "TheCompetitionName" };
            var team = new Team { Name = "TheTeamName" };

            repository.Create(competition);
            new PronoFootDbContext().Teams.Add(team);
            repository.AddTeamToCompetition(competition.CompetitionId, team);

            var sameTeam = new Team { TeamId = team.TeamId, Name = team.Name };
            repository.AddTeamToCompetition(competition.CompetitionId, sameTeam);

            var repositoryToVerify = new CompetitionRepository(new PronoFootDbContext());
            Competition retrievedCompetition = repositoryToVerify.GetCompetitions().First();

            Assert.IsNotNull(competition.Teams);
            Assert.AreEqual(1, competition.Teams.Count);
        }

        [TestMethod]
        public void WhenRemovingTeam_ThenPersistsChange()
        {
            var repository = new CompetitionRepository(new PronoFootDbContext());

            var competition = new Competition { Name = "TheCompetitionName" };
            var team = new Team { Name = "TheTeamName" };

            repository.Create(competition);
            new PronoFootDbContext().Teams.Add(team);
            repository.AddTeamToCompetition(competition.CompetitionId, team);

            repository.RemoveTeamFromCompetition(competition.CompetitionId, team.TeamId);

            var repositoryToVerify = new CompetitionRepository(new PronoFootDbContext());
            Competition retrievedCompetition = repositoryToVerify.GetCompetitions().First();

            Assert.IsNotNull(competition.Teams);
            Assert.AreEqual(0, competition.Teams.Count);
        }

        [TestMethod]
        public void WhenRemovingNotAddedTeam_ThenDoesNothing()
        {
            var repository = new CompetitionRepository(new PronoFootDbContext());

            var competition = new Competition { Name = "TheCompetitionName" };
            var team = new Team { Name = "TheTeamName" };

            repository.Create(competition);
            new PronoFootDbContext().Teams.Add(team);

            repository.RemoveTeamFromCompetition(competition.CompetitionId, team.TeamId);

            var repositoryToVerify = new CompetitionRepository(new PronoFootDbContext());
            Competition retrievedCompetition = repositoryToVerify.GetCompetitions().First();

            Assert.IsNotNull(competition.Teams);
            Assert.AreEqual(0, competition.Teams.Count);
        }

    }
}
