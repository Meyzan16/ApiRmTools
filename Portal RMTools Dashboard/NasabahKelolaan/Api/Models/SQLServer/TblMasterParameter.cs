namespace Api.Models.SQLServer;

public partial class TblMasterParameter
{
    public int Id { get; set; }

    public string? Key { get; set; }

    public string? Value { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public int? DeletedById { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}
