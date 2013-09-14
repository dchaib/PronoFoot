using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using System.Data;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class DayRepository : BaseRepository, IDayRepository
    {
        public DayRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int Create(int editionId, Day day)
        {
            day.EditionId = editionId;
            this.GetDbSet<Day>().Add(day);
            this.UnitOfWork.SaveChanges();
            return day.DayId;
        }

        public Day GetDay(int dayId)
        {
            return this.GetDbSet<Day>()
                            .Where(x => x.DayId == dayId)
                            .Single();
        }

        public IEnumerable<Day> GetDays(int editionId)
        {
            return this.GetDbSet<Day>()
                            .Where(x => x.EditionId == editionId)
                            .ToList();
        }

        public IEnumerable<Day> GetDays(int[] editionIds)
        {
            return this.GetDbSet<Day>()
                            .Where(x => editionIds.Contains(x.EditionId))
                            .ToList();
        }

        public void Update(Day day)
        {
            Day dayToUpdate =
                this.GetDbSet<Day>()
                        .Where(d => d.DayId == day.DayId)
                        .First();

            dayToUpdate.Name = day.Name;
            dayToUpdate.Date = day.Date;

            this.SetEntityState(dayToUpdate, dayToUpdate.DayId == 0
                                                     ? EntityState.Added
                                                     : EntityState.Modified);
            this.UnitOfWork.SaveChanges();
        }
    }
}
