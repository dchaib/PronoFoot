using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Business.Contracts;
using PronoFoot.Security;
using PronoFoot.Business.Models;
using PronoFoot.ViewModels;

namespace PronoFoot.Controllers
{
    public class CompetitionController : BaseController
    {
        private readonly ICompetitionService competitionService;
        private readonly IEditionService editionService;

        public CompetitionController(IUserService userService,
            ICompetitionService competitionService,
            IEditionService editionService,
            IAuthenticationService authenticationService)
            : base(userService, authenticationService)
        {
            this.competitionService = competitionService;
            this.editionService = editionService;
        }

        public ActionResult Details(int id)
        {
            CompetitionModel competition = competitionService.GetCompetition(id);

            if (competition == null)
            {
                return HttpNotFound("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            var editions = editionService.GetEditions(competition.CompetitionId);

            return View(new CompetitionDetailsViewModel
            {
                Competition = competition,
                Editions = editions
            });
        }
    }
}
