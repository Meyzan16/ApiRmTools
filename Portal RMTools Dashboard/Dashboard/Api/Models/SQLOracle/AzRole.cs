using System;
using System.Collections.Generic;

namespace Api.Models.SQLOracle;

public partial class AzRole
{
    public int Appownerid { get; set; }

    public int Applicationid { get; set; }

    public int Roleid { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Script { get; set; }

    public int Profileid { get; set; }

    public string Uniqueid { get; set; } = null!;

    public bool? Ignoresearchscope { get; set; }

    public bool? Isssprole { get; set; }

    public bool? Ishide { get; set; }

    public int Lastmodifiedbytype { get; set; }

    public int Lastmodifiedby { get; set; }

    public string? Rolecode { get; set; }

    public int? Applicationtype { get; set; }

    public string? Ipaddress { get; set; }
}
