using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Specialdayentry
{
    public int Entryid { get; set; }

    public int? Gxdlmsspecialdaystableid { get; set; }

    public int? Entryindex { get; set; }

    public string? Entrydate { get; set; }

    public int? Dayid { get; set; }

    public virtual Gxdlmsspecialdaystable? Gxdlmsspecialdaystable { get; set; }
}
