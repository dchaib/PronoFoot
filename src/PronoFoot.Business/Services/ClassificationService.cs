using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Comparers;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Services
{
    public class ClassificationService : IClassificationService
    { 
        private readonly IForecastRepository forecastRepository;

        public ClassificationService(IForecastRepository forecastRepository)
        {
            this.forecastRepository = forecastRepository;
        }

        public IEnumerable<UserStatistics> GetUserScoresForCompetition(int competitionId)
        {
            var forecasts = forecastRepository.GetForecastsForCompetition(competitionId);
            var userStatistics = GetUserStatistics(forecasts).ToList();
            userStatistics.Sort(new UserStatisticsComparer());
            return userStatistics;
        }

        private static IEnumerable<UserStatistics> GetUserStatistics(IEnumerable<Forecast> forecasts)
        {
            return from forecast in forecasts
                   where forecast.Score.HasValue
                   group forecast by forecast.UserId into g
                   select new UserStatistics(g.Key, g.ToList());
        }

    }
}
