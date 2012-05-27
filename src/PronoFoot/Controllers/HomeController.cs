using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.ViewModels;
using PronoFoot.Security;
using PronoFoot.Business.Models;

namespace PronoFoot.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDayService dayService;
        private readonly IFixtureService fixtureService;
        private readonly IForecastService forecastService;
        private readonly IScoringService scoringService;
        private readonly ICompetitionService competitionService;

        public HomeController(IUserService userService,
            IFixtureService fixtureService,
            IDayService dayService,
            IForecastService forecastService,
            IScoringService scoringService,
            ICompetitionService competitionService,
            IAuthenticationService authenticationService)
            : base(userService, authenticationService)
        {
            this.dayService = dayService;
            this.fixtureService = fixtureService;
            this.forecastService = forecastService;
            this.scoringService = scoringService;
            this.competitionService = competitionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rules()
        {
            var examples = new List<ScoringExample>();
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 2, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 2, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 3, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 3, ForecastAwayGoals = 2 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 4, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 4, ForecastAwayGoals = 3 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 2, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 2 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 1 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 0, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 2, ForecastAwayGoals = 2 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 3, ForecastAwayGoals = 3 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 0 });
            examples.Add(new ScoringExample { ScoreHomeGoals = 1, ScoreAwayGoals = 1, ForecastHomeGoals = 1, ForecastAwayGoals = 2 });

            return View(examples.Select(x => new HomeRulesExampleViewModel
            {
                ScoreHomeGoals = x.ScoreHomeGoals,
                ScoreAwayGoals = x.ScoreAwayGoals,
                ForecastHomeGoals = x.ForecastHomeGoals,
                ForecastAwayGoals = x.ForecastAwayGoals,
                Score = scoringService.GetScore(x.ScoreHomeGoals, x.ScoreAwayGoals, x.ForecastHomeGoals, x.ForecastAwayGoals)
            }));
        }

        private class ScoringExample
        {
            public int ScoreHomeGoals { get; set; }
            public int ScoreAwayGoals { get; set; }
            public int ForecastHomeGoals { get; set; }
            public int ForecastAwayGoals { get; set; }
        }
    }
}
