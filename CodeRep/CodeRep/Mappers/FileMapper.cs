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
    class FileMapper : EntityTypeConfiguration<File>
    {
        public FileMapper()
        {
            this.ToTable("File");

            this.HasKey(s => s.FileId);
            this.Property(s => s.FileId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.FileId).IsRequired();

            this.Property(s => s.FileName).IsRequired();
            this.Property(s => s.FileName).HasMaxLength(255);
            this.Property(s => s.FileName).IsUnicode(false);

            this.Property(s => s.FileType).IsRequired();
            this.Property(s => s.FileType).HasMaxLength(50);
            this.Property(s => s.FileType).IsUnicode(false);

            this.Property(s => s.FileDescription).IsRequired();
            this.Property(s => s.FileDescription).HasMaxLength(1000000000);

            this.Property(s => s.CheckinDate).IsOptional();
            this.Property(s => s.CheckinDate).HasColumnType("smalldatetime");

            //this.HasOptional(c => c.Project).WithMany().Map(s => s.MapKey("ProjectName"));

            this.Property(s => s.path).IsRequired();
            this.Property(s => s.path).HasMaxLength(1000);

            this.Property(s => s.Version).IsOptional();

            this.Property(s => s.FileName).IsOptional();
            this.Property(s => s.FileName).HasMaxLength(500);
            this.Property(s => s.FileName).IsUnicode(false);
        }
    }
}
