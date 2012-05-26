using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Data;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository competitionRepository;

        public CompetitionService(ICompetitionRepository competitionRepository)
        {
            this.competitionRepository = competitionRepository;
        }

        public IList<CompetitionModel> GetCompetitions()
        {
            var competitions = competitionRepository.GetCompetitions();
            var competitionModels = competitions.Select(x => new CompetitionModel(x));
            return competitionModels.ToList();
        }

        public IList<CompetitionModel> GetCurrentCompetitions()
        {
            //TODO Add a parameter to know current competitions
            var competitions = competitionRepository.GetCompetitions();            
            var competitionModels = competitions.Where(x => x.CompetitionId == 2).Select(x => new CompetitionModel(x));
            return competitionModels.ToList();
        }

        public CompetitionModel GetCompetition(int id)
        {
            var competition = competitionRepository.GetCompetition(id);

            var competitionModel = new CompetitionModel(competition);

            return competitionModel;
        }
    }
}
