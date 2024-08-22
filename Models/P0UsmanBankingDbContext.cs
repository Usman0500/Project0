using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project0.Models;

public partial class P0UsmanBankingDbContext : DbContext
{
    public P0UsmanBankingDbContext()
    {
    }

    public P0UsmanBankingDbContext(DbContextOptions<P0UsmanBankingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountInfo> AccountInfos { get; set; }

    public virtual DbSet<NewServiceRequest> NewServiceRequests { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-C0M19OK\\COLUMBUSSERVER;Database=P0_usman_bankingDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountInfo>(entity =>
        {
            entity.HasKey(e => e.AccNo).HasName("PK_acc_no");

            entity.ToTable("account_info");

            entity.Property(e => e.AccNo)
                .ValueGeneratedNever()
                .HasColumnName("acc_no");
            entity.Property(e => e.AccBalance).HasColumnName("acc_balance");
            entity.Property(e => e.AccIsActive).HasColumnName("acc_is_active");
            entity.Property(e => e.AccName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("acc_name");
            entity.Property(e => e.AccType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("acc_type");
            entity.Property(e => e.AccUsername)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("acc_username");

            entity.HasOne(d => d.AccUsernameNavigation).WithMany(p => p.AccountInfos)
                .HasForeignKey(d => d.AccUsername)
                .HasConstraintName("FK_acc_username");
        });

        modelBuilder.Entity<NewServiceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK_request_id");

            entity.ToTable("new_service_request");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.AccNo).HasColumnName("acc_no");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("request_date");
            entity.Property(e => e.RequestStatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("request_status");
            entity.Property(e => e.ServiceType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("service_type");

            entity.HasOne(d => d.AccNoNavigation).WithMany(p => p.NewServiceRequests)
                .HasForeignKey(d => d.AccNo)
                .HasConstraintName("FK_acc_no");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK_transaction_id");

            entity.ToTable("transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AccNo).HasColumnName("acc_no");
            entity.Property(e => e.TransactionAmount).HasColumnName("transaction_amount");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.AccNoNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccNo)
                .HasConstraintName("FK_t_acc_no");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__users__F3DBC5735A5EB77C");

            entity.ToTable("users");

            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UserRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
