﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using System.Data;
using System.Data.Entity;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class CompetitionRepository : BaseRepository, ICompetitionRepository
    {
        public CompetitionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public int Create(Competition competition)
        {
            this.GetDbSet<Competition>().Add(competition);
            this.UnitOfWork.SaveChanges();
            return competition.CompetitionId;
        }

        public Competition GetCompetition(int competitionId)
        {
            return this.GetDbSet<Competition>()
                .Where(x => x.CompetitionId == competitionId)
                .Single();
        }

        public IEnumerable<Competition> GetCompetitions()
        {
            return this.GetDbSet<Competition>()
                .ToList();
        }

        public void Update(Competition competition)
        {
            Competition competitionToUpdate =
                this.GetDbSet<Competition>()
                        .Where(c => c.CompetitionId == competition.CompetitionId)
                        .First();

            competitionToUpdate.Name = competition.Name;
            competitionToUpdate.HasTeamClassification = competition.HasTeamClassification;

            this.SetEntityState(competitionToUpdate, competitionToUpdate.CompetitionId == 0
                                                     ? EntityState.Added
                                                     : EntityState.Modified);
            this.UnitOfWork.SaveChanges();
        }
    }
}
