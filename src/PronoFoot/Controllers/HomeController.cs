using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Data.Model;
using PronoFoot.Data;
using PronoFoot.Data.EntityFramework.Repositories;
using PronoFoot.Data.EntityFramework;
using PronoFoot.Business.Contracts;

namespace PronoFoot.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICompetitionRepository competitionRepository;
        private readonly IDayRepository dayRepository;

        public HomeController(IUserService userServices, ICompetitionRepository competitionRepository, IDayRepository dayRepository)
            : base(userServices)
        {
            this.competitionRepository = competitionRepository;
            this.dayRepository = dayRepository;
        }

        public ActionResult Index()
        {
            int competitionId = 2;
            Competition competition = competitionRepository.GetCompetition(competitionId);

            if (competition == null)
            {
                return HttpNotFound("Il n'y a pas de compétition correspondant à cet identifiant");
            }

            competition.Days = dayRepository.GetDays(competition.CompetitionId).ToList();

            return View(competition);
        }

        public ActionResult Rules()
        {
            return View();
        }
    }
}
