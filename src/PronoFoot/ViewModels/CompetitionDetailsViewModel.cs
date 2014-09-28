using PronoFoot.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class CompetitionDetailsViewModel
    {
        public CompetitionModel Competition { get; set; }
        public IEnumerable<EditionModel> Editions { get; set; }
    }
}