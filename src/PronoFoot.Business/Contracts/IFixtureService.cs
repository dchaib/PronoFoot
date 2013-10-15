using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface IFixtureService
    {
        FixtureModel GetFixture(int fixtureId);
        FixtureModel GetNextFixture(int editionId);
        IEnumerable<FixtureModel> GetFixtures(IEnumerable<int> fixtureIds);
        IEnumerable<FixtureModel> GetFixturesForDay(int dayId);
        IEnumerable<FixtureModel> GetFixturesForEdition(int editionId);
    }
}
