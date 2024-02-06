using System;
using System.Collections.Generic;

namespace Api.Models.SQLServer;

public partial class TblUser
{
    public int Id { get; set; }

    public string? Nama { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? StartLogin { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? IsActive { get; set; }

    public string? Token { get; set; }

    public string? Uid { get; set; }

    public string? SecretKey { get; set; }
}
