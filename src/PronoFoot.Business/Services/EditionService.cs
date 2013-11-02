using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Services
{
    public class EditionService : IEditionService
    {
        private readonly IEditionRepository editionRepository;

        public EditionService(IEditionRepository editionRepository)
        {
            this.editionRepository = editionRepository;
        }
        public IEnumerable<EditionModel> GetEditions(int competitionId)
        {
            var editions = editionRepository.GetEditions(competitionId);
            var editionModels = editions.Select(x => new EditionModel(x));
            return editionModels.ToList();
        }

        public EditionModel GetEdition(int id)
        {
            var edition = editionRepository.GetEdition(id);
            var editionModel = new EditionModel(edition);
            return editionModel;
        }
    }
}
