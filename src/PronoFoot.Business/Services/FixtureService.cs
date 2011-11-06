using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;

namespace PronoFoot.Business.Services
{
    public class FixtureService : IFixtureService
    {
        private readonly IFixtureRepository fixtureRepository;

        public FixtureService(IFixtureRepository fixtureRepository)
        {
            this.fixtureRepository = fixtureRepository;
        }

        public FixtureModel GetFixture(int fixtureId)
        {
            var fixture = fixtureRepository.GetFixture(fixtureId);

            return new FixtureModel(fixture);
        }

        public IEnumerable<FixtureModel> GetFixtures(IEnumerable<int> fixtureIds)
        {
            var fixtures = fixtureRepository.GetFixtures(fixtureIds);

            var fixtureModels = fixtures.Select(x => new FixtureModel(x));

            return fixtureModels.ToList();
        }

        public IEnumerable<FixtureModel> GetFixturesForDay(int dayId)
        {
            var fixtures = fixtureRepository.GetFixturesForDay(dayId);

            var fixtureModels = fixtures.Select(x => new FixtureModel(x));

            return fixtureModels.ToList();
        }

        public IEnumerable<FixtureModel> GetFixturesForCompetition(int competitionId)
        {
            var fixtures = fixtureRepository.GetFixturesForCompetition(competitionId);

            var fixtureModels = fixtures.Select(x => new FixtureModel(x));

            return fixtureModels.ToList();
        }
    }
}
