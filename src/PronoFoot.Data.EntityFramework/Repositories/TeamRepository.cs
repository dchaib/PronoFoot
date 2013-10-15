using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using System.Data;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void Create(Team team)
        {
            this.GetDbSet<Team>().Add(team);
            this.UnitOfWork.SaveChanges();
        }

        public Team GetTeam(int teamId)
        {
            return this.GetDbSet<Team>()
                .Where(x => x.TeamId == teamId)
                .Single();
        }

        public IEnumerable<Team> GetTeams()
        {
            return this.GetDbSet<Team>()
                .ToList();
        }

        public IEnumerable<Team> GetTeamsForEdition(int editionId)
        {
            return this.GetDbSet<Team>()
                .Where(x => x.Editions.Select(y => y.EditionId).Contains(editionId))
                .ToList();
        }

        public void Update(Team team)
        {
            Team teamToUpdate =
                this.GetDbSet<Team>()
                        .Where(t => t.TeamId == team.TeamId)
                        .First();

            teamToUpdate.Name = team.Name;

            this.SetEntityState(teamToUpdate, teamToUpdate.TeamId == 0
                                                     ? EntityState.Added
                                                     : EntityState.Modified);
            this.UnitOfWork.SaveChanges();
        }
    }
}
