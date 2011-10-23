using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class FixtureRepository : BaseRepository, IFixtureRepository
    {
        public FixtureRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void Create(int dayId, Fixture fixture)
        {
            fixture.DayId = dayId;
            this.GetDbSet<Fixture>().Add(fixture);
            this.UnitOfWork.SaveChanges();
        }

        public Fixture GetFixture(int fixtureId)
        {
            return this.GetDbSet<Fixture>()
                            .Where(x => x.FixtureId == fixtureId)
                            .Single();
        }

        public IEnumerable<Fixture> GetFixtures(IEnumerable<int> fixtureIds)
        {
            return this.GetDbSet<Fixture>()
                            .Where(x => fixtureIds.Contains(x.FixtureId))
                            .ToList();
        }

        public IEnumerable<Fixture> GetFixturesForDay(int dayId)
        {
            return this.GetDbSet<Fixture>()
                            .Where(x => x.DayId == dayId)
                            .ToList();
        }
    }
}
