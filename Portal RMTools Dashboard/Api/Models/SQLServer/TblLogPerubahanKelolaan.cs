using System;
using System.Collections.Generic;

namespace Api.Models.SQLServer;

public partial class TblLogPerubahanKelolaan
{
    public int Id { get; set; }

    public string? Cif { get; set; }

    public string? NamaDebitur { get; set; }

    public string? NppRm { get; set; }

    public string? NppBa { get; set; }

    public string? NppRmtransaksi { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedById { get; set; }
}
