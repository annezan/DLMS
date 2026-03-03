using System;
using System.Collections.Generic;

namespace DLMS_MODELS;

public partial class Histories
{
    public int Id { get; set; }
    public int RessourceId { get; set; }
    public int UserId { get; set; }
    public string Ressource { get; set; }
    public string Action { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    //public virtual Users User { get; set; }

}
