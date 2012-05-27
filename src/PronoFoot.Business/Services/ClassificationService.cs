using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public IOrderedEnumerable<UserScoreModel> GetUserScoresForCompetition(int competitionId)
        {
            var forecasts = forecastRepository.GetForecastsForCompetition(competitionId);
            return GetUserScores(forecasts)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.NumberOfExactForecasts)
                .ThenByDescending(x => x.NumberOfCloseForecasts);
        }

        private static IEnumerable<UserScoreModel> GetUserScores(IEnumerable<Forecast> forecasts)
        {
            return from forecast in forecasts
                   where forecast.Score.HasValue
                   group forecast by forecast.UserId into g
                   select new UserScoreModel
                   {
                       UserId = g.Key,
                       Score = g.Sum(f => f.Score.Value),
                       NumberOfExactForecasts = g.Count(f => f.Score.Value == 3),
                       NumberOfCloseForecasts = g.Count(f => f.Score.Value == 2),
                       NumberOfForecastsWithExactDifference = g.Count(f => f.Score.Value == 1.5),
                       NumberOfCorrect1N2Forecasts = g.Count(f => f.Score.Value == 1),
                       NumberOfWrongForecasts = g.Count(f => f.Score.Value == 0)
                   };
        }
    }
}
