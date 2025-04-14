using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class Technic
{
    public int IdTechnic { get; set; }

    public string? DescriptionTechnic { get; set; }

    public string? PhotoTechnic { get; set; }

    public int? IdModelTechnic { get; set; }

    public virtual ModelTechnic? IdModelTechnicNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
