using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsassociationlogicalname
{
    public int Associationid { get; set; }

    public int Objectid { get; set; }

    public int? Version { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public int? Clientsap { get; set; }

    public int? Serversap { get; set; }

    public int? Associationstatus { get; set; }

    public string? Securitysetupreference { get; set; }

    public int? Multipleassociationviews { get; set; }

    public string? Secret { get; set; }

    public string? Users { get; set; }

    public virtual ICollection<Applicationcontext> Applicationcontexts { get; set; } = new List<Applicationcontext>();

    public virtual ICollection<Authenticationmechanism> Authenticationmechanisms { get; set; } = new List<Authenticationmechanism>();

    public virtual ICollection<Gxdlmsassociationobjectlist> Gxdlmsassociationobjectlists { get; set; } = new List<Gxdlmsassociationobjectlist>();

    public virtual Objects Object { get; set; } = null!;

    public virtual ICollection<Xdlmscontextinfo> Xdlmscontextinfos { get; set; } = new List<Xdlmscontextinfo>();
}
