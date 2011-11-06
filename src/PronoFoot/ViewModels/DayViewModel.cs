using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PronoFoot.ViewModels
{
    public class DayViewModel
    {
        public int DayId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool CanBeForecast { get; set; }
        public bool ForecastMadeByCurrentUser { get; set; }
    }
}