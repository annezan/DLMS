using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Dayactionspassive
{
    public int Actionpassiveid { get; set; }

    public int? Dayprofilepassiveid { get; set; }

    public string? Start { get; set; }

    public string? Logicalname { get; set; }

    public int? Selector { get; set; }

    public virtual Dayprofiletablepassive? Dayprofilepassive { get; set; }
}
