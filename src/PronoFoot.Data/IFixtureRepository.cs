using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface IFixtureRepository
    {
        void Create(int dayId, Fixture fixture);

        Fixture GetFixture(int fixtureId);
        IEnumerable<Fixture> GetFixtures(IEnumerable<int> fixtureIds);
        IEnumerable<Fixture> GetFixturesForDay(int dayId);
        IEnumerable<Fixture> GetFixturesForEdition(int editionId);

        void Save(Fixture fixture);
    }
}
