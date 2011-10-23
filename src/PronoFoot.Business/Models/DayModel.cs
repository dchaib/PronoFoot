using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Models
{
    public class DayModel
    {
        private Day day;

        public int DayId { get { return this.day.DayId; } }
        public string Name { get { return this.day.Name; } }
        public DateTime Date { get { return this.day.Date; } }
        public int CompetitionId { get { return this.day.CompetitionId; } }

        public DayModel(Day day)
        {
            this.day = day;
        }
    }
}
