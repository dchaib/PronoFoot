using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Services
{
    public class DayServices : IDayServices
    {
        private readonly IDayRepository dayRepository;
        private readonly IFixtureRepository fixtureRepository;

        public DayServices(IDayRepository dayRepository, IFixtureRepository fixtureRepository)
        {
            this.dayRepository = dayRepository;
            this.fixtureRepository = fixtureRepository;
        }

        public DayModel GetDay(int id)
        {
            var day = dayRepository.GetDay(id);

            var dayModel = new DayModel(day);

            return dayModel;
        }

        public void Update(DayModel day, IEnumerable<FixtureModel> fixtures)
        {
            var dbDay = dayRepository.GetDay(day.DayId);

            dbDay.Date = day.Date;
            dbDay.Name = day.Name;

            dayRepository.Update(dbDay);

            var dbFixtures = fixtureRepository.GetFixturesForDay(day.DayId);

            foreach (var fixture in fixtures)
            {
                var dbFixture = dbFixtures.FirstOrDefault(x => x.FixtureId == fixture.FixtureId) ?? new Fixture();

                dbFixture.DayId = dbDay.DayId;
                dbFixture.Date = fixture.Date;
                dbFixture.HomeTeamId = fixture.HomeTeamId;
                dbFixture.AwayTeamId = fixture.AwayTeamId;
                dbFixture.HomeTeamGoals = fixture.HomeTeamGoals;
                dbFixture.AwayTeamGoals = fixture.AwayTeamGoals;

                fixtureRepository.Save(dbFixture);                
            }
        }
    }
}
