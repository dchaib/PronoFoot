using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using PronoFoot.Data.Model;

namespace PronoFoot.Data.EntityFramework
{
    public class PronoFootDbContext : DbContext, IUnitOfWork
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SetUpFixtureEntity(modelBuilder);
        }

        private static void SetUpFixtureEntity(DbModelBuilder modelBuilder)
        {
            //TODO Foreign keys
            //modelBuilder.Entity<Fixture>().HasRequired(f => f.HomeTeam)
            //    .WithMany()
            //    .Map(m => m.MapKey("HomeTeamId"))
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<Fixture>().HasRequired(f => f.AwayTeam)
            //    .WithMany()
            //    .Map(m => m.MapKey("AwayTeamId"))
            //    .WillCascadeOnDelete(false);
        }

        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Forecast> Forecasts { get; set; }
        public DbSet<User> Users { get; set; }

        void IUnitOfWork.SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
