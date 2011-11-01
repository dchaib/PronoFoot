using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class DayFormViewModel
    {
        public int DayId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public IList<FixtureViewModel> Fixtures { get; set; }


    }
}