using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface IForecastService
    {
        IEnumerable<ForecastModel> GetForecastsForDay(int dayId);
        IEnumerable<ForecastModel> GetForecastsForDayUser(int dayId, int userId);
        IDictionary<int, int> GetForecastCountByDayForEditionUser(int editionId, int userId);

        void SaveForecasts(IEnumerable<ForecastModel> forecasts);

        void DeleteForecasts(IEnumerable<int> forecastIds);
    }
}
