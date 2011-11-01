using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using System.Data;

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

        public void Save(Fixture fixture)
        {
            if (fixture == null)
                throw new ArgumentNullException("fixture");

            //var dbSet = this.GetDbSet<Fixture>();
            this.SetEntityState(fixture, fixture.FixtureId == 0 ? EntityState.Added : EntityState.Modified);
            this.UnitOfWork.SaveChanges();
        }

    }
}
