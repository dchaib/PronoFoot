using PronoFoot.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class EditionRepository : BaseRepository, IEditionRepository
    {
        public EditionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        public void AddTeamToEdition(int editionId, Team team)
        {
            Edition edition = this.GetDbSet<Edition>()
                .Include("Teams")
                .Where(x => x.EditionId == editionId)
                .First();

            if (edition.Teams.Any(x => x.TeamId == team.TeamId))
                return;

            edition.Teams.Add(team);

            this.UnitOfWork.SaveChanges();
        }

        public void RemoveTeamFromEdition(int editionId, int teamId)
        {
            Edition edition = this.GetDbSet<Edition>()
                .Include("Teams")
                .Where(c => c.CompetitionId == editionId)
                .First();

            var teamToRemove = edition.Teams.Where(x => x.TeamId == teamId).FirstOrDefault();

            if (teamToRemove == null)
                return;

            edition.Teams.Remove(teamToRemove);

            this.UnitOfWork.SaveChanges();
        }
        
        public IEnumerable<Edition> GetEditions(int competitionId)
        {
            return this.GetDbSet<Edition>()
                .Where(x => x.CompetitionId == competitionId)
                .ToList();
        }


        public Edition GetEdition(int id)
        {
            return this.GetDbSet<Edition>()
                .Where(x => x.EditionId == id)
                .Single();
        }
    }
}
