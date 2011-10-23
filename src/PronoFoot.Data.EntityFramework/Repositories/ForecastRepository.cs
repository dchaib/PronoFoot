using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using System.Data;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class ForecastRepository : BaseRepository, IForecastRepository
    {
        public ForecastRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Forecast GetForecast(int forecastId)
        {
            return this.GetDbSet<Forecast>()
                            .Where(x => x.ForecastId == forecastId)
                            .Single();
        }

        public Forecast GetForecast(int fixtureId, int userId)
        {
            return this.GetDbSet<Forecast>()
                            .Where(x => x.FixtureId == fixtureId && x.UserId == userId)
                            .SingleOrDefault();
        }

        public IEnumerable<Forecast> GetForecasts(IEnumerable<int> forecastIds)
        {
            return this.GetDbSet<Forecast>().Where(x => forecastIds.Contains(x.ForecastId)).ToList();
        }

        public IEnumerable<Forecast> GetForecastsForDay(int dayId)
        {
            //TODO Find a better way, not using FixtureDbSet?
            var q = from fixture in this.GetDbSet<Fixture>()
                    from forecast in fixture.Forecasts
                    where fixture.DayId == dayId
                    select forecast;
            return q.ToList();
        }

        public IEnumerable<Forecast> GetForecastsForDayUser(int dayId, int userId)
        {
            //TODO Find a better way, not using FixtureDbSet?
            var q = from fixture in this.GetDbSet<Fixture>()
                    from forecast in fixture.Forecasts
                    where fixture.DayId == dayId && forecast.UserId == userId
                    select forecast;
            return q.ToList();
        }

        public void Save(IEnumerable<Forecast> forecasts)
        {
            var dbSet = this.GetDbSet<Forecast>();
            foreach (var forecast in forecasts)
            {
                dbSet.Attach(forecast);
                this.SetEntityState(forecast, forecast.ForecastId == 0 ? EntityState.Added : EntityState.Modified);
            }
            this.UnitOfWork.SaveChanges();
        }

        public void Delete(IEnumerable<int> forecastIds)
        {
            var dbSet = this.GetDbSet<Forecast>();
            foreach (var forecastId in forecastIds)
            {
                var forecast = this.GetForecast(forecastId);
                dbSet.Remove(forecast);
            }
            this.UnitOfWork.SaveChanges();
        }
    }
}
