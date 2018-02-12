using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository
{
    public class RepositoryContextMigrationConfiguration : DbMigrationsConfiguration<RepositoryContext>
    {
        public RepositoryContextMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
            
        }

        protected override void Seed(RepositoryContext context)
        {
            new RepositorySeeder(context).Seed();
        }

    }
}
