using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations
{
    public class HealthRecordConfiguration:IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<HealthRecord> builder)
        {
          builder.Property(x=>x.BloodType)
                .HasMaxLength(6);
            builder.Property(x => x.Note)
                .HasMaxLength(500);
            

        }
    }
}
