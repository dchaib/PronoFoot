using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.Models.Common
{
    public class MenuModel
    {
        public IList<CompetitionModel> Competitions { get; set; }

        public MenuModel()
        {
            this.Competitions = new List<CompetitionModel>();
        }

        public class CompetitionModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}