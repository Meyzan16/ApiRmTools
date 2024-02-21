using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.SQLServer;

public partial class dbRmTools_Context : DbContext
{
    public dbRmTools_Context(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    private readonly IConfiguration _configuration;
    public dbRmTools_Context(IConfiguration configuration, DbContextOptions<dbRmTools_Context> options)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<TblJwtRepository> TblJwtRepositories { get; set; }

    public virtual DbSet<TblLogActivity> TblLogActivities { get; set; }

    public virtual DbSet<TblLogPerubahanKelolaan> TblLogPerubahanKelolaans { get; set; }

    public virtual DbSet<TblMasterFile> TblMasterFiles { get; set; }

    public virtual DbSet<TblMasterLookup> TblMasterLookups { get; set; }

    public virtual DbSet<TblMasterNasabahKelolaan> TblMasterNasabahKelolaans { get; set; }

    public virtual DbSet<TblMasterNasabahKelolaanForOd> TblMasterNasabahKelolaanForOds { get; set; }

    public virtual DbSet<TblMasterNavigation> TblMasterNavigations { get; set; }

    public virtual DbSet<TblMasterNavigationAssignment> TblMasterNavigationAssignments { get; set; }

    public virtual DbSet<TblMasterParameter> TblMasterParameters { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("dbSqlServer"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblJwtRepository>(entity =>
        {
            entity.ToTable("Tbl_JwtRepository");

            entity.Property(e => e.ClientIp).HasColumnName("ClientIP");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Token).HasColumnType("text");
        });

        modelBuilder.Entity<TblLogActivity>(entity =>
        {
            entity.ToTable("Tbl_LogActivity");

            entity.Property(e => e.ActionTime).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasColumnName("IP");
            entity.Property(e => e.Os).HasColumnName("OS");
        });

        modelBuilder.Entity<TblLogPerubahanKelolaan>(entity =>
        {
            entity.ToTable("Tbl_LogPerubahanKelolaan");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.NamaDebitur).HasColumnName("Nama_debitur");
            entity.Property(e => e.NppBa)
                .HasMaxLength(50)
                .HasColumnName("Npp_BA");
            entity.Property(e => e.NppRm)
                .HasMaxLength(50)
                .HasColumnName("Npp_RM");
            entity.Property(e => e.NppRmtransaksi)
                .HasMaxLength(50)
                .HasColumnName("Npp_RMTransaksi");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterFile>(entity =>
        {
            entity.ToTable("Tbl_MasterFile");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedBy_Id");
            entity.Property(e => e.Ext)
                .HasMaxLength(50)
                .HasColumnName("ext");
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.FileSize).HasMaxLength(50);
            entity.Property(e => e.FullPath).HasMaxLength(500);
            entity.Property(e => e.Path).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterLookup>(entity =>
        {
            entity.ToTable("Tbl_MasterLookup");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedBy_Id");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterNasabahKelolaan>(entity =>
        {
            entity.ToTable("Tbl_MasterNasabahKelolaan");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CifParent)
                .HasMaxLength(500)
                .HasColumnName("CIF_Parent");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById)
                .HasColumnType("datetime")
                .HasColumnName("CreatedBy_Id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DeletedById)
                .HasColumnType("datetime")
                .HasColumnName("DeletedBy_Id");
            entity.Property(e => e.FileId).HasColumnName("File_Id");
            entity.Property(e => e.KodeUnit).HasColumnName("Kode_unit");
            entity.Property(e => e.NamaNasabahDebitur).HasColumnName("Nama_nasabah_debitur");
            entity.Property(e => e.NamaParentNasabah).HasColumnName("Nama_parent_nasabah");
            entity.Property(e => e.NamaUnit)
                .HasMaxLength(500)
                .HasColumnName("Nama_unit");
            entity.Property(e => e.NppBa)
                .HasMaxLength(50)
                .HasColumnName("Npp_BA");
            entity.Property(e => e.NppRm)
                .HasMaxLength(50)
                .HasColumnName("Npp_RM");
            entity.Property(e => e.NppRmtransaksi)
                .HasMaxLength(50)
                .HasColumnName("Npp_RMTransaksi");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterNasabahKelolaanForOd>(entity =>
        {
            entity.ToTable("Tbl_MasterNasabahKelolaanForODS");

            entity.Property(e => e.Cif)
                .HasMaxLength(50)
                .HasColumnName("CIF");
            entity.Property(e => e.CifParent)
                .HasMaxLength(500)
                .HasColumnName("CIF_Parent");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById)
                .HasColumnType("datetime")
                .HasColumnName("CreatedBy_Id");
            entity.Property(e => e.KodeUnit).HasColumnName("Kode_unit");
            entity.Property(e => e.NamaNasabahDebitur).HasColumnName("Nama_nasabah_debitur");
            entity.Property(e => e.NamaParentNasabah).HasColumnName("Nama_parent_nasabah");
            entity.Property(e => e.NamaUnit)
                .HasMaxLength(500)
                .HasColumnName("Nama_unit");
            entity.Property(e => e.NppBa)
                .HasMaxLength(50)
                .HasColumnName("Npp_BA");
            entity.Property(e => e.NppRm)
                .HasMaxLength(50)
                .HasColumnName("Npp_RM");
            entity.Property(e => e.NppRmtransaksi)
                .HasMaxLength(50)
                .HasColumnName("Npp_RMTransaksi");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterNavigation>(entity =>
        {
            entity.ToTable("Tbl_MasterNavigation");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedBy_Id");
            entity.Property(e => e.IconClass).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterNavigationAssignment>(entity =>
        {
            entity.ToTable("Tbl_MasterNavigationAssignment");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedBy_Id");
            entity.Property(e => e.NavigationId).HasColumnName("Navigation_Id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblMasterParameter>(entity =>
        {
            entity.ToTable("Tbl_MasterParameter");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedById).HasColumnName("CreatedBy_Id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("Deleted_at");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedBy_Id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedBy_Id");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.ToTable("Tbl_User");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.StartLogin).HasColumnType("datetime");
            entity.Property(e => e.Token).HasColumnType("text");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
