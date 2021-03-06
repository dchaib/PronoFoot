﻿using System;
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

        public FixtureModel GetNextFixture(int editionId)
        {
            var query = from f in fixtureRepository.GetFixturesForEdition(editionId)
                        let fm = new FixtureModel(f)
                        where fm.CanBeForecast
                        orderby fm.Date
                        select fm;

            return query.FirstOrDefault();
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

        public IEnumerable<FixtureModel> GetFixturesForEdition(int editionId)
        {
            var fixtures = fixtureRepository.GetFixturesForEdition(editionId);

            var fixtureModels = fixtures.Select(x => new FixtureModel(x));

            return fixtureModels.ToList();
        }
    }
}
