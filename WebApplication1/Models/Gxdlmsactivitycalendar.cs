using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsactivitycalendar
{
    public int Gxdlmsactivitycalendarid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public string? Calendarnameactive { get; set; }

    public string? Calendarnamepassive { get; set; }

    public string? Time { get; set; }

    public DateTime? Capturetime { get; set; }

    public virtual ICollection<Dayprofiletableactive> Dayprofiletableactives { get; set; } = new List<Dayprofiletableactive>();

    public virtual ICollection<Dayprofiletablepassive> Dayprofiletablepassives { get; set; } = new List<Dayprofiletablepassive>();

    public virtual Objects Object { get; set; } = null!;

    public virtual ICollection<Seasonprofileactive> Seasonprofileactives { get; set; } = new List<Seasonprofileactive>();

    public virtual ICollection<Seasonprofilepassive> Seasonprofilepassives { get; set; } = new List<Seasonprofilepassive>();

    public virtual ICollection<Weekprofiletableactive> Weekprofiletableactives { get; set; } = new List<Weekprofiletableactive>();

    public virtual ICollection<Weekprofiletablepassive> Weekprofiletablepassives { get; set; } = new List<Weekprofiletablepassive>();
}
