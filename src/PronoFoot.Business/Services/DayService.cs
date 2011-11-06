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
    public class DayService : IDayService
    {
        private readonly IDayRepository dayRepository;
        private readonly IFixtureRepository fixtureRepository;
        private readonly IForecastRepository forecastRepository;
        private readonly IScoringService scoringService;

        public DayService(IDayRepository dayRepository,
            IFixtureRepository fixtureRepository,
            IForecastRepository forecastRepository,
            IScoringService scoringService)
        {
            this.dayRepository = dayRepository;
            this.fixtureRepository = fixtureRepository;
            this.forecastRepository = forecastRepository;
            this.scoringService = scoringService;
        }

        public DayModel GetDay(int id)
        {
            var day = dayRepository.GetDay(id);

            var dayModel = new DayModel(day);

            return dayModel;
        }

        public IEnumerable<DayModel> GetDaysForCompetition(int competitionId)
        {
            var q = from day in dayRepository.GetDays(competitionId)
                    select new DayModel(day);
            return q.ToList();
        }

        public int Create(DayModel day, IEnumerable<FixtureModel> fixtures)
        {
            var dbDay = new Day();

            dbDay.CompetitionId = day.CompetitionId;
            dbDay.Date = day.Date;
            dbDay.Name = day.Name;

            int dayId = dayRepository.Create(day.CompetitionId, dbDay);
            
            foreach (var fixture in fixtures)
            {
                var dbFixture = new Fixture();

                dbFixture.DayId = dayId;
                dbFixture.Date = fixture.Date;
                dbFixture.HomeTeamId = fixture.HomeTeamId;
                dbFixture.AwayTeamId = fixture.AwayTeamId;
                dbFixture.HomeTeamGoals = fixture.HomeTeamGoals;
                dbFixture.AwayTeamGoals = fixture.AwayTeamGoals;

                fixtureRepository.Create(dayId, dbFixture);
            }

            return dayId;
        }

        public void Update(DayModel day, IEnumerable<FixtureModel> fixtures)
        {
            var dbDay = dayRepository.GetDay(day.DayId);

            dbDay.Date = day.Date;
            dbDay.Name = day.Name;

            dayRepository.Update(dbDay);

            var dbFixtures = fixtureRepository.GetFixturesForDay(day.DayId);

            foreach (var fixture in fixtures)
            {
                var dbFixture = dbFixtures.FirstOrDefault(x => x.FixtureId == fixture.FixtureId) ?? new Fixture();

                dbFixture.DayId = dbDay.DayId;
                dbFixture.Date = fixture.Date;
                dbFixture.HomeTeamId = fixture.HomeTeamId;
                dbFixture.AwayTeamId = fixture.AwayTeamId;
                dbFixture.HomeTeamGoals = fixture.HomeTeamGoals;
                dbFixture.AwayTeamGoals = fixture.AwayTeamGoals;

                fixtureRepository.Save(dbFixture);

                if (fixture.FixtureId > 0)
                {
                    var forecasts = forecastRepository.GetForecastsForFixture(fixture.FixtureId);
                    if (forecasts.Count() > 0)
                    {
                        if (fixture.HomeTeamGoals.HasValue && fixture.AwayTeamGoals.HasValue)
                        {
                            foreach (var forecast in forecasts)
                            {
                                forecast.Score = scoringService.GetScore(fixture.HomeTeamGoals.Value, fixture.AwayTeamGoals.Value, forecast.HomeTeamGoals, forecast.AwayTeamGoals);
                            }
                        }
                        else
                        {
                            foreach (var forecast in forecasts)
                            {
                                forecast.Score = null;
                            }
                        }
                        forecastRepository.Save(forecasts);
                    }
                }
            }
        }
    }
}
