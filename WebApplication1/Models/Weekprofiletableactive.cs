using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Weekprofiletableactive
{
    public int Weekprofileactiveid { get; set; }

    public int? Gxdlmsactivitycalendarid { get; set; }

    public string? Name { get; set; }

    public int? Monday { get; set; }

    public int? Tuesday { get; set; }

    public int? Wednesday { get; set; }

    public int? Thursday { get; set; }

    public int? Friday { get; set; }

    public int? Saturday { get; set; }

    public int? Sunday { get; set; }

    public virtual Gxdlmsactivitycalendar? Gxdlmsactivitycalendar { get; set; }
}
