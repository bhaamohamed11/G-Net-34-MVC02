using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace GymManagementSystem.DAL.Configurations
{
    public class MemberShipConfigurations:IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Property(x=>x.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");
            builder.HasOne(x=>x.Plan)
                .WithMany(c => c.PlanMembers)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Member)
                .WithMany(c => c.MemberPlans)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
