using CodeRepository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRepository.Mappers
{
    class UserAccessMapper : EntityTypeConfiguration<UserAccess>
    {
        public UserAccessMapper()
        {
            this.ToTable("UserAccess");

            this.HasKey(s => s.AccessId);
            this.Property(s => s.AccessId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.AccessId).IsRequired();

            this.Property(s => s.Role).IsRequired();
            this.Property(s => s.Role).HasMaxLength(255);
            this.Property(s => s.Role).IsUnicode(false);

            this.HasRequired(c => c.Project).WithMany().Map(s => s.MapKey("ProjectId"));
            this.HasRequired(c => c.User).WithMany().Map(s => s.MapKey("UserId"));
        }
    }

}
