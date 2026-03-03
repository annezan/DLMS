using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gxdlmsgprssetup
{
    public int Gxdlmsgprssetupid { get; set; }

    public int Objectid { get; set; }

    public string? Access { get; set; }

    public string? Methodaccess { get; set; }

    public string? Apn { get; set; }

    public string? Pincode { get; set; }

    public virtual ICollection<Gxdlmsgprssetupdefaultqo> Gxdlmsgprssetupdefaultqos { get; set; } = new List<Gxdlmsgprssetupdefaultqo>();

    public virtual ICollection<Gxdlmsgprssetuprequestedqo> Gxdlmsgprssetuprequestedqos { get; set; } = new List<Gxdlmsgprssetuprequestedqo>();

    public virtual Objects Object { get; set; } = null!;
}
