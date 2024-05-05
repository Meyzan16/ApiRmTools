using System;
using System.Collections.Generic;

namespace Api.Models.SQLServer;

public partial class TblLogActivity
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Path { get; set; }

    public string? Status { get; set; }

    public string? Message { get; set; }

    public DateTime? ActionTime { get; set; }

    public string? Browser { get; set; }

    public string? Ip { get; set; }

    public string? Os { get; set; }

    public string? ClientInfo { get; set; }
}
