using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface IClassificationService
    {
        IEnumerable<UserStatistics> GetUserScoresForEdition(int editionId);
    }
}
