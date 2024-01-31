using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.SQLServer;

public partial class dbRmTools_Context : DbContext
{
    public dbRmTools_Context()
    {
    }

    public dbRmTools_Context(DbContextOptions<dbRmTools_Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TblLogPerubahanKelolaan> TblLogPerubahanKelolaans { get; set; }

    public virtual DbSet<TblMasterFile> TblMasterFiles { get; set; }

    public virtual DbSet<TblMasterLookup> TblMasterLookups { get; set; }

    public virtual DbSet<TblMasterNasabahKelolaan> TblMasterNasabahKelolaans { get; set; }

    public virtual DbSet<TblMasterNavigation> TblMasterNavigations { get; set; }

    public virtual DbSet<TblMasterNavigationAssignment> TblMasterNavigationAssignments { get; set; }

    public virtual DbSet<TblMasterParameter> TblMasterParameters { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1435;Initial Catalog=db_rmtools_meyzan;User ID=sa;Password=bni46SQL;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.RoleId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Role_Id");
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
