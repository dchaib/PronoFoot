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
        IEnumerable<Forecast> GetForecastsForDay(int dayId);
        IEnumerable<Forecast> GetForecastsForDayUser(int dayId, int userId);

        void Save(IEnumerable<Forecast> forecasts);

        void Delete(IEnumerable<int> forecastIds);
    }
}