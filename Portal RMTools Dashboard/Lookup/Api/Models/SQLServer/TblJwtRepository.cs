using System;
using System.Collections.Generic;

namespace Api.Models.SQLServer;

public partial class TblJwtRepository
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? ClientIp { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public bool? IsStop { get; set; }

    public string? Hostname { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}
