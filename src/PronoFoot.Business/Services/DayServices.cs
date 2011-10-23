using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Business.Models;
using PronoFoot.Data;

namespace PronoFoot.Business.Services
{
    public class DayServices : IDayServices
    {
        private readonly IDayRepository dayRepository;

        public DayServices(IDayRepository dayRepository)
        {
            this.dayRepository = dayRepository;
        }

        public DayModel GetDay(int id)
        {
            var day = dayRepository.GetDay(id);

            var dayModel = new DayModel(day);

            return dayModel;
        }
    }
}
