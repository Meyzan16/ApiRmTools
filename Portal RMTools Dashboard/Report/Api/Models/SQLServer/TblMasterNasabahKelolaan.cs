using System;
using System.Collections.Generic;

namespace Api.Models.SQLServer;

public partial class TblMasterNasabahKelolaan
{
    public int Id { get; set; }

    public int? KodeUnit { get; set; }

    public string? NamaUnit { get; set; }

    public string? CifParent { get; set; }

    public string? NamaParentNasabah { get; set; }

    public string? Cif { get; set; }

    public string? NamaNasabahDebitur { get; set; }

    public string? NppRm { get; set; }

    public string? NppBa { get; set; }

    public string? NppRmtransaksi { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? CreatedById { get; set; }

    public DateTime? UpdatedById { get; set; }

    public DateTime? DeletedById { get; set; }

    public int? FileId { get; set; }

    public bool? StatusData { get; set; }

    public string? Komentar { get; set; }
}
