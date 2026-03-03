using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Dayprofiletableactive
{
    public int Dayprofileactiveid { get; set; }

    public int? Gxdlmsactivitycalendarid { get; set; }

    public int? Dayid { get; set; }

    public virtual Gxdlmsactivitycalendar? Gxdlmsactivitycalendar { get; set; }
}
