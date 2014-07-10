using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Business.Contracts
{
    public interface IScoringService
    {
        Rating GetRating(int homeGoals, int awayGoals, int forecastHomeGoals, int forecastAwayGoals);
        double GetScore(Rating rating);
    }
}
