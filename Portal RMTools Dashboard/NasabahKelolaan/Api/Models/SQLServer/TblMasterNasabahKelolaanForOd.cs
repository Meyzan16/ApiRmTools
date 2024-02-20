namespace Api.Models.SQLServer;

public partial class TblMasterNasabahKelolaanForOd
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

    public DateTime? CreatedById { get; set; }

    public DateTime? UpdatedById { get; set; }
}
