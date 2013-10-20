using PronoFoot.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Business.Contracts
{
    public interface IEditionService
    {
        IEnumerable<EditionModel> GetEditions(int competitionId);

        EditionModel GetEdition(int id);
    }
}
