using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.Models.Common;
using PronoFoot.Models.Competition;

namespace PronoFoot.Controllers
{
    public class CommonController : Controller
    {
        private readonly ICompetitionService competitionService;

        public CommonController(ICompetitionService competitionService)
        {
            this.competitionService = competitionService;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var competitions = competitionService.GetCompetitions();

            var model = new MenuModel();
            foreach (var competition in competitions)
            {
                var competitionModel = new MenuModel.CompetitionModel
                {
                    Id = competition.CompetitionId,
                    Name = competition.Name
                };

                model.Competitions.Add(competitionModel);
            }

            return PartialView(model);
        }

    }
}
