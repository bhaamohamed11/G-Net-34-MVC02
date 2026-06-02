using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations
{
    public class TrainerConfiguration:GymUserConfiguration<Trainer>,IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GETDATE()");


        }
    }
}
