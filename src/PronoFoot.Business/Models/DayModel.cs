using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class DayModel
    {
        public int DayId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Coefficient { get; set; }
        public int CompetitionId { get; set; }

        public DayModel()
        {
        }

        public DayModel(Day day)
        {
            this.DayId = day.DayId;
            this.CompetitionId = day.CompetitionId;
            this.Name = day.Name;
            this.Date = day.Date;
            this.Coefficient = day.Coefficient;
        }
    }
}
