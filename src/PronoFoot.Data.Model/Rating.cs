using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Data.Model
{
    public enum Rating
    {
        Wrong = 0,
        CorrectOutcome = 1,
        CorrectDifference = 2,
        Close = 3,
        Exact = 4
    }
}
