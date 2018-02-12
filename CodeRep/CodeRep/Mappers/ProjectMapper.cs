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
    class ProjectMapper : EntityTypeConfiguration<Project>
    {
        public ProjectMapper()
        {
            this.ToTable("Project");

            this.HasKey(s => s.ProjectId);
            this.Property(s => s.ProjectId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.ProjectId).IsRequired();

            this.Property(s => s.ProjectName).IsRequired();
            this.Property(s => s.ProjectName).HasMaxLength(255);
            this.Property(s => s.ProjectName).IsUnicode(false);

            this.Property(s => s.ProjectLanguage).IsOptional();
            this.Property(s => s.ProjectLanguage).HasMaxLength(50);
            this.Property(s => s.ProjectLanguage).IsUnicode(false);

            this.Property(s => s.ProjectDescription).IsOptional();
            this.Property(s => s.ProjectDescription).HasMaxLength(1000);

            this.Property(s => s.CreationDate).IsOptional();
            this.Property(s => s.CreationDate).HasColumnType("smalldatetime");

            this.Property(s => s.LastModifiedDate).IsOptional();
            this.Property(s => s.LastModifiedDate).HasColumnType("smalldatetime");

            this.Property(s => s.UserId).IsRequired();
        }
    }
}
