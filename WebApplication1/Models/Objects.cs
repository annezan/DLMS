using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Objects
{
    public int Objectid { get; set; }

    public string? Objecttype { get; set; }

    public string? Ln { get; set; }

    public string? Description { get; set; }

    public string? Description2 { get; set; }

    public virtual ICollection<Gxdlmsactionschedule> Gxdlmsactionschedules { get; set; } = new List<Gxdlmsactionschedule>();

    public virtual ICollection<Gxdlmsactivitycalendar> Gxdlmsactivitycalendars { get; set; } = new List<Gxdlmsactivitycalendar>();

    public virtual ICollection<Gxdlmsassociationlogicalname> Gxdlmsassociationlogicalnames { get; set; } = new List<Gxdlmsassociationlogicalname>();

    public virtual ICollection<Gxdlmsautoconnect> Gxdlmsautoconnects { get; set; } = new List<Gxdlmsautoconnect>();

    public virtual ICollection<Gxdlmsclock> Gxdlmsclocks { get; set; } = new List<Gxdlmsclock>();

    public virtual ICollection<Gxdlmsdatum> Gxdlmsdata { get; set; } = new List<Gxdlmsdatum>();

    public virtual ICollection<Gxdlmsdemandregister> Gxdlmsdemandregisters { get; set; } = new List<Gxdlmsdemandregister>();

    public virtual ICollection<Gxdlmsdisconnectcontrol> Gxdlmsdisconnectcontrols { get; set; } = new List<Gxdlmsdisconnectcontrol>();

    public virtual ICollection<Gxdlmsextendedregister> Gxdlmsextendedregisters { get; set; } = new List<Gxdlmsextendedregister>();

    public virtual ICollection<Gxdlmsgprssetup> Gxdlmsgprssetups { get; set; } = new List<Gxdlmsgprssetup>();

    public virtual ICollection<Gxdlmsgsmdiagnostic> Gxdlmsgsmdiagnostics { get; set; } = new List<Gxdlmsgsmdiagnostic>();

    public virtual ICollection<Gxdlmsiechdlcsetup> Gxdlmsiechdlcsetups { get; set; } = new List<Gxdlmsiechdlcsetup>();

    public virtual ICollection<Gxdlmsieclocalportsetup> Gxdlmsieclocalportsetups { get; set; } = new List<Gxdlmsieclocalportsetup>();

    public virtual ICollection<Gxdlmslimiter> Gxdlmslimiters { get; set; } = new List<Gxdlmslimiter>();

    public virtual ICollection<Gxdlmsregister> Gxdlmsregisters { get; set; } = new List<Gxdlmsregister>();

    public virtual ICollection<Gxdlmssapassignment> Gxdlmssapassignments { get; set; } = new List<Gxdlmssapassignment>();

    public virtual ICollection<Gxdlmsscripttable> Gxdlmsscripttables { get; set; } = new List<Gxdlmsscripttable>();

    public virtual ICollection<Gxdlmssecuritysetup> Gxdlmssecuritysetups { get; set; } = new List<Gxdlmssecuritysetup>();

    public virtual ICollection<Gxdlmsspecialdaystable> Gxdlmsspecialdaystables { get; set; } = new List<Gxdlmsspecialdaystable>();

    public virtual ICollection<Objectrelation> ObjectrelationChildobjects { get; set; } = new List<Objectrelation>();

    public virtual ICollection<Objectrelation> ObjectrelationParentobjects { get; set; } = new List<Objectrelation>();
}
