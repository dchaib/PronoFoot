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
    public class DayRepositoryFixture
    {
        private Competition testCompetition;
        //TODO test retrieve fixture
        [TestInitialize]
        public void Initialize()
        {
            DatabaseTestUtility.DropCreateDatabase();
            this.testCompetition = new Competition()
            {
                Name = "TestCompetition"
            };

            var competitionRepository = new CompetitionRepository(this.GetUnitOfWork());
            competitionRepository.Create(this.testCompetition);
        }

        [TestMethod]
        public void WhenAddingDay_ThenPersistsChange()
        {
            var repository = new DayRepository(this.GetUnitOfWork());

            string dayName = "TheDayName";
            DateTime dayDate = DateTime.Today;
            var newDay = new Day()
            {
                Name = dayName,
                Date = dayDate
            };

            repository.Create(testCompetition.CompetitionId, newDay);

            var actualDays = new PronoFootDbContext().Days.ToList();

            Assert.AreEqual(1, actualDays.Count());
            Assert.AreEqual(testCompetition.CompetitionId, actualDays[0].CompetitionId);
            Assert.AreEqual(dayName, actualDays[0].Name);
            Assert.AreEqual(dayDate, actualDays[0].Date);
        }

        [TestMethod]
        public void WhenAddingDay_ThenUpdatesDayId()
        {
            var repository = new DayRepository(this.GetUnitOfWork());

            string dayName = "TheDayName";
            DateTime dayDate = DateTime.Today;
            var newDay = new Day()
            {
                Name = dayName,
                Date = dayDate
            };

            repository.Create(testCompetition.CompetitionId, newDay);

            List<Day> actualDays = new PronoFootDbContext().Days.ToList();

            Assert.AreEqual(1, actualDays.Count());
            Assert.AreEqual(1, actualDays[0].DayId);
        }

        [TestMethod]
        public void WhenAddingDay_ThenReturnsPopulatedDay()
        {
            var repository = new DayRepository(this.GetUnitOfWork());

            string dayName = "TheDayName";
            DateTime dayDate = DateTime.Today;
            var newDay = new Day()
            {
                Name = dayName,
                Date = dayDate
            };

            repository.Create(testCompetition.CompetitionId, newDay);

            Assert.IsNotNull(newDay);
            Assert.AreEqual(testCompetition.CompetitionId, newDay.CompetitionId);
            Assert.AreEqual(dayName, newDay.Name);
            Assert.AreEqual(dayDate, newDay.Date);
        }

        [TestMethod]
        public void WhenGettingAllDaysForNewCompetition_ThenReturnsEmptyCollection()
        {
            var repository = new DayRepository(this.GetUnitOfWork());

            var days = repository.GetDays(this.testCompetition.CompetitionId);

            Assert.IsNotNull(days);
            Assert.AreEqual(0, days.Count());
        }

        [TestMethod]
        public void WhenGettingAllDays_ThenReturnsAllDays()
        {
            var repository = new DayRepository(this.GetUnitOfWork());

            var day1 = new Day()
            {
                Name = "Day1",
                Date = DateTime.Now
            };
            repository.Create(this.testCompetition.CompetitionId, day1);

            var day2 = new Day()
            {
                Name = "Day2",
                Date = DateTime.Now
            };
            repository.Create(this.testCompetition.CompetitionId, day2);


            var days = repository.GetDays(this.testCompetition.CompetitionId);

            Assert.IsNotNull(days);
            Assert.AreEqual(2, days.Count());
        }

        [TestMethod]
        public void WhenModifyingDay_ThenPersistsChange()
        {
            var repository = new DayRepository(this.GetUnitOfWork());

            var day = new Day { Name = "DayName", Date = DateTime.Today };
            repository.Create(this.testCompetition.CompetitionId, day);

            // I use a new context and repository to verify the data was stored
            var repositoryForUpdate = new DayRepository(this.GetUnitOfWork());

            var retrievedDay = repositoryForUpdate.GetDays(this.testCompetition.CompetitionId).First();

            var updatedName = "Updated Day Name";
            var updatedDate = DateTime.Now;
            retrievedDay.Name = updatedName;
            retrievedDay.Date = updatedDate;
            repositoryForUpdate.Update(retrievedDay);
            int updatedDayId = retrievedDay.DayId;

            var repositoryForVerifaction = new DayRepository(this.GetUnitOfWork());
            Day updatedDay = repositoryForVerifaction.GetDay(updatedDayId);

            Assert.AreEqual(updatedName, updatedDay.Name);
            Assert.AreEqual(updatedDate.ToString(), updatedDay.Date.ToString());
        }

        private IUnitOfWork GetUnitOfWork()
        {
            return new PronoFootDbContext();
        }
    }
}
