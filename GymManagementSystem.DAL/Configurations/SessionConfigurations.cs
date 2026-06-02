using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations
{
    public class SessionConfigurations:IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder) 
        {
            builder.ToTable(tb =>
            { 
            tb.HasCheckConstraint("SessionCapacityConstraint", "Capacity between 1 and 25");
                tb.HasCheckConstraint("SessionTimeConstraint", "EndDate >StartDate");

               
            });
            builder.HasOne(x => x.Trainer)
                   .WithMany(c => c.TrainerSessions)
                   .HasForeignKey(x => x.TrainerId);
            builder.HasOne(x => x.Category)
                .WithMany(c => c.Sessions)
                .HasForeignKey(x => x.CategoryId);
        }   
        
    }
}
