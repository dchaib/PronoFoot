using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace PronoFoot.Models
{
    public static class MappingExtensions
    {
        #region Competition
        public static PronoFoot.Business.Models.CompetitionModel ToEntity(this PronoFoot.Models.Competition.CompetitionModel model)
        {
            return Mapper.Map<PronoFoot.Models.Competition.CompetitionModel, PronoFoot.Business.Models.CompetitionModel>(model);
        }

        public static PronoFoot.Models.Competition.CompetitionModel ToModel(this PronoFoot.Business.Models.CompetitionModel entity)
        {
            return Mapper.Map<PronoFoot.Business.Models.CompetitionModel, PronoFoot.Models.Competition.CompetitionModel>(entity);
        }
        #endregion
    }
}