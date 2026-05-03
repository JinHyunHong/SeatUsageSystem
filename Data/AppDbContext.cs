using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Models.Entities;
using System.IO;

namespace SeatUsageSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Area> Areas => Set<Area>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Usage> Usages => Set<Usage>();
        public DbSet<InOutHistory> InoutHistories => Set<InOutHistory>();
        public DbSet<CommonCodeManage> CommonCodeManages => Set<CommonCodeManage>();
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbPath = Path.Combine(AppContext.BaseDirectory, "Database", "SeatUsage.db"); // 실행위치 달라도 가능
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // area
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("area");

                entity.HasKey(e => e.AreaId);

                entity.Property(e => e.AreaId).HasColumnName("area_id");
                entity.Property(e => e.DisplayName).HasColumnName("display_name");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            // seat
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("seat");

                entity.HasKey(e => e.SeatId);

                entity.Property(e => e.SeatId).HasColumnName("seat_id");
                entity.Property(e => e.AreaId).HasColumnName("area_id");
                entity.Property(e => e.UsageStateCd).HasColumnName("usage_state_cd");
                entity.Property(e => e.DisplayName).HasColumnName("display_name");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.Area)
                      .WithMany()
                      .HasForeignKey(e => e.AreaId);
            });

            // member
            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.HasKey(e => e.MemberId);

                entity.Property(e => e.MemberId).HasColumnName("member_id");
                entity.Property(e => e.MemberName).HasColumnName("member_name");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            // usage
            modelBuilder.Entity<Usage>(entity =>
            {
                entity.ToTable("usage");

                entity.HasKey(e => e.UsageId);

                entity.Property(e => e.UsageId).HasColumnName("usage_id");
                entity.Property(e => e.MemberId).HasColumnName("member_id");
                entity.Property(e => e.SeatId).HasColumnName("seat_id");
                entity.Property(e => e.StartAt).HasColumnName("start_at");
                entity.Property(e => e.EndAt).HasColumnName("end_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.Member)
                      .WithMany()
                      .HasForeignKey(e => e.MemberId);

                entity.HasOne(e => e.Seat)
                      .WithMany()
                      .HasForeignKey(e => e.SeatId);
            });

            // inout_history
            modelBuilder.Entity<InOutHistory>(entity =>
            {
                entity.ToTable("inout_history");

                entity.HasKey(e => new { e.InOutYmd, e.InOutSeq });

                entity.Property(e => e.InOutYmd).HasColumnName("inout_ymd");
                entity.Property(e => e.InOutSeq).HasColumnName("inout_seq");
                entity.Property(e => e.UsageId).HasColumnName("usage_id");
                entity.Property(e => e.InOutCd).HasColumnName("inout_cd");
                entity.Property(e => e.InOutTime).HasColumnName("inout_time");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.Usage)
                      .WithMany()
                      .HasForeignKey(e => e.UsageId);
            });

            // common_code_manage
            modelBuilder.Entity<CommonCodeManage>(entity =>
            {
                entity.ToTable("common_code_manage");

                entity.HasKey(e => new
                {
                    e.LargeGroup,
                    e.MiddleGroup,
                    e.SmallGroup
                });

                entity.Property(e => e.LargeGroup).HasColumnName("large_group");
                entity.Property(e => e.MiddleGroup).HasColumnName("middle_group");
                entity.Property(e => e.SmallGroup).HasColumnName("small_group");

                entity.Property(e => e.ConfigValue1).HasColumnName("config_value1");
                entity.Property(e => e.ConfigValue2).HasColumnName("config_value2");
                entity.Property(e => e.ConfigValue3).HasColumnName("config_value3");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });
        }
    }
}