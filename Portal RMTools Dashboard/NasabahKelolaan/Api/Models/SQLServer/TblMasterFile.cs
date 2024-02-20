namespace Api.Models.SQLServer;

public partial class TblMasterFile
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public string? FileSize { get; set; }

    public string? Ext { get; set; }

    public string? Path { get; set; }

    public string? FullPath { get; set; }

    public int? DataSuccess { get; set; }

    public int? DataFailed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public int? DeletedById { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }
}
