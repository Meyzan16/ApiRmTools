using System;
using System.Collections.Generic;

namespace Api.Models.SQLServer;

public partial class TblMasterLookup
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public int? Value { get; set; }

    public int? OrdeyBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    public int? DeletedById { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}
