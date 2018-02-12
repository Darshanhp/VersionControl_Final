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
    class UserMapper : EntityTypeConfiguration<User>
    {
        public UserMapper()
        {
            this.ToTable("User");

            this.HasKey(s => s.UserId);
            this.Property(s => s.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.UserId).IsRequired();

            this.Property(s => s.Email).IsRequired();
            this.Property(s => s.Email).HasMaxLength(255);
            this.Property(s => s.Email).IsUnicode(false);

            this.Property(s => s.UserName).IsRequired();
            this.Property(s => s.UserName).HasMaxLength(50);
            this.Property(s => s.UserName).IsUnicode(false);

            this.Property(s => s.Password).IsRequired();
            this.Property(s => s.Password).HasMaxLength(255);

            this.Property(s => s.FirstName).IsRequired();
            this.Property(s => s.FirstName).HasMaxLength(50);

            this.Property(s => s.LastName).IsRequired();
            this.Property(s => s.LastName).HasMaxLength(50);

            this.Property(s => s.Gender).IsOptional();

            this.Property(s => s.DateOfBirth).IsRequired();
            this.Property(s => s.DateOfBirth).HasColumnType("smalldatetime");

            this.Property(s => s.RegistrationDate).IsOptional();
            this.Property(s => s.RegistrationDate).HasColumnType("smalldatetime");

            this.Property(s => s.LastLoginDate).IsOptional();
            this.Property(s => s.LastLoginDate).HasColumnType("smalldatetime");

            this.Property(s => s.Description).IsRequired();
            this.Property(s => s.Description).HasMaxLength(1000);

        }
    }

}
