using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Seasonprofileactive
{
    public int Seasonprofileactiveid { get; set; }

    public int? Gxdlmsactivitycalendarid { get; set; }

    public string? Name { get; set; }

    public string? Start { get; set; }

    public string? Weekname { get; set; }

    public virtual Gxdlmsactivitycalendar? Gxdlmsactivitycalendar { get; set; }
}
