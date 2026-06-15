using GymManagementSystem.DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(x=>x.Id);
            builder.Property(x => x.CreatedAt).HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            #region Relationships
            builder.HasOne(x => x.Session)
                .WithMany(c => c.SessionMembers)
                .HasForeignKey(x => x.SessionId);

            builder.HasOne(x => x.Member)
                .WithMany(c => c.MemberSessions)
                .HasForeignKey(x => x.MemberId);

            builder.HasKey(x => new { x.MemberId, x.SessionId });

            #endregion


        }
    }
}
