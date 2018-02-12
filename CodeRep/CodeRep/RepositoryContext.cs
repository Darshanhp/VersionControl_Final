using CodeRep.Entities;
using CodeRep.Mappers;
using CodeRepository.Entities;
using CodeRepository.Mappers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext() :
            base("CodeRepositoryConnectionNew")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RepositoryContext, RepositoryContextMigrationConfiguration>());
        }

        public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<UserAccess> UserAccess { get; set; }
        public DbSet<CoAuthor> CoAuthor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMapper());
            modelBuilder.Configurations.Add(new ProjectMapper());
            modelBuilder.Configurations.Add(new FileMapper());
            modelBuilder.Configurations.Add(new UserAccessMapper());
            modelBuilder.Configurations.Add(new CoAuthorMapper());
            base.OnModelCreating(modelBuilder);
        }
    }
}
