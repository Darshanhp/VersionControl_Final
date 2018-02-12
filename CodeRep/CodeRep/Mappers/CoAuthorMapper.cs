using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeRep.Entities;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeRep.Mappers 
{
    public class CoAuthorMapper : EntityTypeConfiguration<CoAuthor>
    {
        public CoAuthorMapper()
        {
            this.ToTable("CoAuthor");

            this.HasKey(s => s.ID);
            this.Property(s => s.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.ID).IsRequired();

            this.Property(s => s.Project).IsRequired();
            this.Property(s => s.Project).HasMaxLength(255);
            this.Property(s => s.Project).IsUnicode(false);

            this.Property(s => s.Author).IsRequired();
            this.Property(s => s.Author).HasMaxLength(50);
            this.Property(s => s.Author).IsUnicode(false);

            this.Property(s => s.CoAuhtor).IsRequired();
            this.Property(s => s.CoAuhtor).HasMaxLength(1000000000);
        }
    }
}
