using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Dayprofiletablepassive
{
    public int Dayprofilepassiveid { get; set; }

    public int? Gxdlmsactivitycalendarid { get; set; }

    public int? Dayid { get; set; }

    public virtual ICollection<Dayactionspassive> Dayactionspassives { get; set; } = new List<Dayactionspassive>();

    public virtual Gxdlmsactivitycalendar? Gxdlmsactivitycalendar { get; set; }
}
