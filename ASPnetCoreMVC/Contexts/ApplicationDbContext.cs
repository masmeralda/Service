using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASPnetCoreMVC.Contexts;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<HistoryOfChangingOrderStatus> HistoryOfChangingOrderStatuses { get; set; }

    public virtual DbSet<ModelTechnic> ModelTechnics { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PositionOfASpecialist> PositionOfASpecialists { get; set; }

    public virtual DbSet<RoleUser> RoleUsers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Specialist> Specialists { get; set; }

    public virtual DbSet<Technic> Technics { get; set; }

    public virtual DbSet<TypeTechnic> TypeTechnics { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<RoleUser> Roles { get; set; } 


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Service;Username=postgres;Password=000");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.ApartmentNumberClient)
                .HasMaxLength(10)
                .HasColumnName("apartment_number_client");
            entity.Property(e => e.EmailClient)
                .HasMaxLength(255)
                .HasColumnName("email_client");
            entity.Property(e => e.FirstNameClient)
                .HasMaxLength(255)
                .HasColumnName("first_name_client");
            entity.Property(e => e.HouseClient)
                .HasMaxLength(10)
                .HasColumnName("house_client");
            entity.Property(e => e.LastNameClient)
                .HasMaxLength(255)
                .HasColumnName("last_name_client");
            entity.Property(e => e.PatronymicClient)
                .HasMaxLength(255)
                .HasColumnName("patronymic_client");
            entity.Property(e => e.PhoneClient)
                .HasMaxLength(20)
                .HasColumnName("phone_client");
            entity.Property(e => e.StreetClient)
                .HasMaxLength(255)
                .HasColumnName("street_client");
        });

        modelBuilder.Entity<HistoryOfChangingOrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdHistory).HasName("history_of_changing_order_statuses_pkey");

            entity.ToTable("history_of_changing_order_statuses");

            entity.Property(e => e.IdHistory).HasColumnName("id_history");
            entity.Property(e => e.ChangedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changed_at");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdSpecialist).HasColumnName("id_specialist");
            entity.Property(e => e.StatusHistory)
                .HasMaxLength(50)
                .HasColumnName("status_history");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.HistoryOfChangingOrderStatuses)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("history_of_changing_order_statuses_id_order_fkey");

            entity.HasOne(d => d.IdSpecialistNavigation).WithMany(p => p.HistoryOfChangingOrderStatuses)
                .HasForeignKey(d => d.IdSpecialist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("history_of_changing_order_statuses_id_specialist_fkey");
        });

        modelBuilder.Entity<ModelTechnic>(entity =>
        {
            entity.HasKey(e => e.IdModelTechnic).HasName("model_technic_pkey");

            entity.ToTable("model_technic");

            entity.Property(e => e.IdModelTechnic).HasColumnName("id_model_technic");
            entity.Property(e => e.IdTypeTechnic).HasColumnName("id_type_technic");
            entity.Property(e => e.NameModelTechnic)
                .HasMaxLength(100)
                .HasColumnName("name_model_technic");

            entity.HasOne(d => d.IdTypeTechnicNavigation).WithMany(p => p.ModelTechnics)
                .HasForeignKey(d => d.IdTypeTechnic)
                .HasConstraintName("model_technic_id_type_technic_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("order_pkey");

            entity.ToTable("order");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.CommentOrder).HasColumnName("comment_order");
            entity.Property(e => e.CompletionDateOrder)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completion_date_order");
            entity.Property(e => e.CreationDateOrder)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("creation_date_order");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.IdSpecialist).HasColumnName("id_specialist");
            entity.Property(e => e.IdTechnic).HasColumnName("id_technic");
            entity.Property(e => e.StatusOrder)
                .HasMaxLength(50)
                .HasColumnName("status_order");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_id_client_fkey");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_id_service_fkey");

            entity.HasOne(d => d.IdSpecialistNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdSpecialist)
                .HasConstraintName("order_id_specialist_fkey");

            entity.HasOne(d => d.IdTechnicNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdTechnic)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_id_technic_fkey");
        });

        modelBuilder.Entity<PositionOfASpecialist>(entity =>
        {
            entity.HasKey(e => e.IdPosition).HasName("position_of_a_specialist_pkey");

            entity.ToTable("position_of_a_specialist");

            entity.Property(e => e.IdPosition).HasColumnName("id_position");
            entity.Property(e => e.NamePosition)
                .HasMaxLength(255)
                .HasColumnName("name_position");
        });

        modelBuilder.Entity<RoleUser>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("role_user_pkey");

            entity.ToTable("role_user");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.IdService).HasColumnName("id_service");
            entity.Property(e => e.ExecutionTimeService).HasColumnName("execution_time_service");
            entity.Property(e => e.NameService)
                .HasMaxLength(255)
                .HasColumnName("name_service");
            entity.Property(e => e.PriceService)
                .HasPrecision(10, 2)
                .HasColumnName("price_service");
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.IdSpecialist).HasName("specialist_pkey");

            entity.ToTable("specialist");

            entity.Property(e => e.IdSpecialist).HasColumnName("id_specialist");
            entity.Property(e => e.EmailSpecialist)
                .HasMaxLength(255)
                .HasColumnName("email_specialist");
            entity.Property(e => e.FirstNameSpecialist)
                .HasMaxLength(255)
                .HasColumnName("first_name_specialist");
            entity.Property(e => e.IdPosition).HasColumnName("id_position");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.LastNameSpecialist)
                .HasMaxLength(255)
                .HasColumnName("last_name_specialist");
            entity.Property(e => e.PatronymicSpecialist)
                .HasMaxLength(255)
                .HasColumnName("patronymic_specialist");
            entity.Property(e => e.PhoneNumberSpecialist)
                .HasMaxLength(20)
                .HasColumnName("phone_number_specialist");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.Specialists)
                .HasForeignKey(d => d.IdPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialist_id_position_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Specialists)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialist_id_user_fkey");
        });

        modelBuilder.Entity<Technic>(entity =>
        {
            entity.HasKey(e => e.IdTechnic).HasName("technic_pkey");

            entity.ToTable("technic");

            entity.Property(e => e.IdTechnic).HasColumnName("id_technic");
            entity.Property(e => e.DescriptionTechnic).HasColumnName("description_technic");
            entity.Property(e => e.IdModelTechnic).HasColumnName("id_model_technic");
            entity.Property(e => e.PhotoTechnic).HasColumnName("photo_technic");

            entity.HasOne(d => d.IdModelTechnicNavigation).WithMany(p => p.Technics)
                .HasForeignKey(d => d.IdModelTechnic)
                .HasConstraintName("technic_id_model_technic_fkey");
        });

        modelBuilder.Entity<TypeTechnic>(entity =>
        {
            entity.HasKey(e => e.IdTypeTechnic).HasName("type_technic_pkey");

            entity.ToTable("type_technic");

            entity.Property(e => e.IdTypeTechnic).HasColumnName("id_type_technic");
            entity.Property(e => e.NameTypeTechnic)
                .HasMaxLength(100)
                .HasColumnName("name_type_technic");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.LoginUser)
                .HasMaxLength(255)
                .HasColumnName("login_user");
            entity.Property(e => e.PasswordUser)
                .HasMaxLength(255)
                .HasColumnName("password_user");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("user_id_role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
