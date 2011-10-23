using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using System.Data;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class CompetitionRepository : BaseRepository, ICompetitionRepository
    {
        public CompetitionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void Create(Competition competition)
        {
            this.GetDbSet<Competition>().Add(competition);
            this.UnitOfWork.SaveChanges();
        }

        public Competition GetCompetition(int competitionId)
        {
            return this.GetDbSet<Competition>()
                .Include("Teams")
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

            this.SetEntityState(competitionToUpdate, competitionToUpdate.CompetitionId == 0
                                                     ? EntityState.Added
                                                     : EntityState.Modified);
            this.UnitOfWork.SaveChanges();
        }

        public void AddTeamToCompetition(int competitionId, Team team)
        {
            Competition competition = this.GetDbSet<Competition>()
                .Include("Teams")
                .Where(c => c.CompetitionId == competitionId)
                .First();

            if (competition.Teams.Any(x => x.TeamId == team.TeamId))
                return;

            competition.Teams.Add(team);

            this.UnitOfWork.SaveChanges();
        }

        public void RemoveTeamFromCompetition(int competitionId, int teamId)
        {
            Competition competition = this.GetDbSet<Competition>()
                .Include("Teams")
                .Where(c => c.CompetitionId == competitionId)
                .First();

            var teamToRemove = competition.Teams.Where(x => x.TeamId == teamId).FirstOrDefault();

            if (teamToRemove == null)
                return;

            competition.Teams.Remove(teamToRemove);

            this.UnitOfWork.SaveChanges();
        }
    }
}
