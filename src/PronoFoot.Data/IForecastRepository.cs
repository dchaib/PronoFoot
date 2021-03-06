﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface IForecastRepository
    {
        Forecast GetForecast(int fixtureId, int userId);

        IEnumerable<Forecast> GetForecasts(IEnumerable<int> forecastIds);
        IEnumerable<Forecast> GetForecastsForEdition(int editionId);
        IEnumerable<Forecast> GetForecastsForDay(int dayId);
        IEnumerable<Forecast> GetForecastsForDayUser(int dayId, int userId);
        IEnumerable<Forecast> GetForecastsForFixture(int fixtureId);
        IDictionary<int, int> GetForecastCountByDayForEditionUser(int editionId, int userId);

        void Save(IEnumerable<Forecast> forecasts);

        void Delete(IEnumerable<int> forecastIds);        
    }
}
