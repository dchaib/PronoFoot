using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Data;
using PronoFoot.Business.Models;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Services
{
    public class ForecastService : IForecastService
    {
        private readonly IForecastRepository forecastRepository;
        private readonly IFixtureService fixtureService;

        public ForecastService(IForecastRepository forecastRepository, IFixtureService fixtureService)
        {
            this.forecastRepository = forecastRepository;
            this.fixtureService = fixtureService;
        }

        public IEnumerable<ForecastModel> GetForecastsForDay(int dayId)
        {
            var forecasts = forecastRepository.GetForecastsForDay(dayId);

            var forecastModels = forecasts.Select(x => new ForecastModel(x));

            return forecastModels.ToList();
        }

        public IEnumerable<ForecastModel> GetForecastsForDayUser(int dayId, int userId)
        {
            var forecasts = forecastRepository.GetForecastsForDayUser(dayId, userId);

            var forecastModels = forecasts.Select(x => new ForecastModel(x));

            return forecastModels.ToList();
        }

        public IDictionary<int, int> GetForecastCountByDayForCompetitionUser(int competitionId, int userId)
        {
            return forecastRepository.GetForecastCountByDayForCompetitionUser(competitionId, userId);
        }

        public void SaveForecasts(IEnumerable<ForecastModel> forecasts)
        {
            var forecastsToSave = new List<Forecast>();
            foreach (var forecast in forecasts)
            {
                var fixture = fixtureService.GetFixture(forecast.FixtureId);
                if (fixture.CanBeForecast)
                {
                    Forecast f = forecastRepository.GetForecast(forecast.FixtureId, forecast.UserId);
                    if (f == null)
                    {
                        f = new Forecast()
                            {
                                UserId = forecast.UserId,
                                FixtureId = forecast.FixtureId
                            };
                    }
                    f.HomeTeamGoals = forecast.HomeTeamGoals;
                    f.AwayTeamGoals = forecast.AwayTeamGoals;
                    forecastsToSave.Add(f);
                }
            }

            forecastRepository.Save(forecastsToSave);
        }

        public void DeleteForecasts(IEnumerable<int> forecastIds)
        {
            var forecasts = forecastRepository.GetForecasts(forecastIds);
            var fixtures = fixtureService.GetFixtures(forecasts.Select(x => x.FixtureId));


            var forecastIdsToDelete = from forecast in forecasts
                                      where fixtures.Single(x => x.FixtureId == forecast.FixtureId).CanBeForecast
                                      select forecast.ForecastId;

            forecastRepository.Delete(forecastIdsToDelete);
        }
    }
}
