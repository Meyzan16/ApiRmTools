namespace Api.Models.SQLServer;

public partial class TblMasterNavigation
{
    public int Id { get; set; }

    public int? Type { get; set; }

    public string? Name { get; set; }

    public string? Route { get; set; }

    public int? Order { get; set; }

    public int? Visible { get; set; }

    public int? ParentNavigationId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public int? DeletedById { get; set; }

    public bool? IsDeleted { get; set; }

    public string? IconClass { get; set; }
}
