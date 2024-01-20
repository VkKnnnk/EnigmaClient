using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class CompClub_dbContext : DbContext
    {
        public CompClub_dbContext()
        {
        }

        public CompClub_dbContext(DbContextOptions<CompClub_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Authdatum> Authdata { get; set; }
        public virtual DbSet<Computer> Computers { get; set; }
        public virtual DbSet<Keyboard> Keyboards { get; set; }
        public virtual DbSet<Monitor> Monitors { get; set; }
        public virtual DbSet<Mouse> Mice { get; set; }
        public virtual DbSet<Processor> Processors { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Tariff> Tariffs { get; set; }
        public virtual DbSet<TariffAllowance> TariffAllowances { get; set; }
        public virtual DbSet<TypeTariff> TypeTariffs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VideoCard> VideoCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;database=CompClub_db;user id=root;password=2v3nqgeG#", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.24-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Authdatum>(entity =>
            {
                entity.HasKey(e => e.IdAuth)
                    .HasName("PRIMARY");

                entity.ToTable("authdata");

                entity.Property(e => e.IdAuth).HasColumnName("idAuth");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("phone");
            });

            modelBuilder.Entity<Computer>(entity =>
            {
                entity.HasKey(e => e.IdComp)
                    .HasName("PRIMARY");

                entity.ToTable("computers");

                entity.HasIndex(e => e.IdKeyboard, "idKeyboard");

                entity.HasIndex(e => e.IdMonitor, "idMonitor");

                entity.HasIndex(e => e.IdMouse, "idMouse");

                entity.HasIndex(e => e.IdProcessor, "idProcessor");

                entity.HasIndex(e => e.IdTypeTariffs, "idTariff");

                entity.HasIndex(e => e.IdVideoCard, "idVideo_card");

                entity.Property(e => e.IdComp)
                    .ValueGeneratedNever()
                    .HasColumnName("idComp");

                entity.Property(e => e.IdKeyboard).HasColumnName("idKeyboard");

                entity.Property(e => e.IdMonitor).HasColumnName("idMonitor");

                entity.Property(e => e.IdMouse).HasColumnName("idMouse");

                entity.Property(e => e.IdProcessor).HasColumnName("idProcessor");

                entity.Property(e => e.IdTypeTariffs).HasColumnName("idTypeTariffs");

                entity.Property(e => e.IdVideoCard).HasColumnName("idVideo_card");

                entity.Property(e => e.Ram).HasColumnName("RAM");

                entity.HasOne(d => d.IdKeyboardNavigation)
                    .WithMany(p => p.Computers)
                    .HasForeignKey(d => d.IdKeyboard)
                    .HasConstraintName("computers_ibfk_5");

                entity.HasOne(d => d.IdMonitorNavigation)
                    .WithMany(p => p.Computers)
                    .HasForeignKey(d => d.IdMonitor)
                    .HasConstraintName("computers_ibfk_4");

                entity.HasOne(d => d.IdMouseNavigation)
                    .WithMany(p => p.Computers)
                    .HasForeignKey(d => d.IdMouse)
                    .HasConstraintName("computers_ibfk_6");

                entity.HasOne(d => d.IdProcessorNavigation)
                    .WithMany(p => p.Computers)
                    .HasForeignKey(d => d.IdProcessor)
                    .HasConstraintName("computers_ibfk_2");

                entity.HasOne(d => d.IdTypeTariffsNavigation)
                    .WithMany(p => p.Computers)
                    .HasForeignKey(d => d.IdTypeTariffs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("computers_ibfk_1");

                entity.HasOne(d => d.IdVideoCardNavigation)
                    .WithMany(p => p.Computers)
                    .HasForeignKey(d => d.IdVideoCard)
                    .HasConstraintName("computers_ibfk_3");
            });

            modelBuilder.Entity<Keyboard>(entity =>
            {
                entity.HasKey(e => e.IdKeyboard)
                    .HasName("PRIMARY");

                entity.ToTable("keyboards");

                entity.Property(e => e.IdKeyboard).HasColumnName("idKeyboard");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("model");
            });

            modelBuilder.Entity<Monitor>(entity =>
            {
                entity.HasKey(e => e.IdMonitor)
                    .HasName("PRIMARY");

                entity.ToTable("monitors");

                entity.Property(e => e.IdMonitor).HasColumnName("idMonitor");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("model");

                entity.Property(e => e.Resolution)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("resolution");
            });

            modelBuilder.Entity<Mouse>(entity =>
            {
                entity.HasKey(e => e.IdMouse)
                    .HasName("PRIMARY");

                entity.ToTable("mice");

                entity.Property(e => e.IdMouse).HasColumnName("idMouse");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("model");
            });

            modelBuilder.Entity<Processor>(entity =>
            {
                entity.HasKey(e => e.IdProcessor)
                    .HasName("PRIMARY");

                entity.ToTable("processors");

                entity.Property(e => e.IdProcessor).HasColumnName("idProcessor");

                entity.Property(e => e.CoreAmount)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("core_amount");

                entity.Property(e => e.Frequency)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("frequency");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("model");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.IdSession)
                    .HasName("PRIMARY");

                entity.ToTable("sessions");

                entity.HasIndex(e => e.IdComputer, "idComputer");

                entity.HasIndex(e => e.IdUser, "idUser");

                entity.HasIndex(e => e.IdTariff, "sessions_ibfk_2");

                entity.Property(e => e.IdSession).HasColumnName("idSession");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.EndSession)
                    .HasColumnType("datetime")
                    .HasColumnName("end_session");

                entity.Property(e => e.IdComputer).HasColumnName("idComputer");

                entity.Property(e => e.IdTariff).HasColumnName("idTariff");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.StartSession)
                    .HasColumnType("datetime")
                    .HasColumnName("start_session");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.IdComputerNavigation)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.IdComputer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sessions_ibfk_3");

                entity.HasOne(d => d.IdTariffNavigation)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.IdTariff)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sessions_ibfk_2");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sessions_ibfk_1");
            });

            modelBuilder.Entity<Tariff>(entity =>
            {
                entity.HasKey(e => e.IdTariff)
                    .HasName("PRIMARY");

                entity.ToTable("tariff");

                entity.HasIndex(e => e.IdTypeTariffs, "idTypeTariffs");

                entity.Property(e => e.IdTariff).HasColumnName("idTariff");

                entity.Property(e => e.AppearanceDuration).HasColumnName("appearance_duration");

                entity.Property(e => e.AppearanceHour).HasColumnName("appearance_hour");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.FixPrice).HasColumnName("fix_price");

                entity.Property(e => e.IdTypeTariffs).HasColumnName("idTypeTariffs");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.IdTypeTariffsNavigation)
                    .WithMany(p => p.Tariffs)
                    .HasForeignKey(d => d.IdTypeTariffs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tariff_ibfk_1");
            });

            modelBuilder.Entity<TariffAllowance>(entity =>
            {
                entity.HasKey(e => e.IdAllowance)
                    .HasName("PRIMARY");

                entity.ToTable("tariff_allowance");

                entity.Property(e => e.IdAllowance).HasColumnName("idAllowance");

                entity.Property(e => e.AllowancePercent).HasColumnName("allowance_percent");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TypeTariff>(entity =>
            {
                entity.HasKey(e => e.IdTypeTariffs)
                    .HasName("PRIMARY");

                entity.ToTable("type_tariffs");

                entity.Property(e => e.IdTypeTariffs).HasColumnName("idTypeTariffs");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PriceHour).HasColumnName("price_hour");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.IdAuth, "idAuth");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.AuthStatus).HasColumnName("auth_status");

                entity.Property(e => e.AvatarImg)
                    .HasMaxLength(150)
                    .HasColumnName("avatar_img");

                entity.Property(e => e.Cash).HasColumnName("cash");

                entity.Property(e => e.IdAuth).HasColumnName("idAuth");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.PersonalDiscount).HasColumnName("personal_discount");

                entity.HasOne(d => d.IdAuthNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdAuth)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("users_ibfk_1");
            });

            modelBuilder.Entity<VideoCard>(entity =>
            {
                entity.HasKey(e => e.IdVideoCard)
                    .HasName("PRIMARY");

                entity.ToTable("video_cards");

                entity.Property(e => e.IdVideoCard).HasColumnName("idVideo_card");

                entity.Property(e => e.Memory).HasColumnName("memory");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("model");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
