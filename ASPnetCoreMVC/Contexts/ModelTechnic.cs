using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class ModelTechnic
{
    public int IdModelTechnic { get; set; }

    public string NameModelTechnic { get; set; } = null!;

    public int IdTypeTechnic { get; set; }

    public virtual TypeTechnic IdTypeTechnicNavigation { get; set; } = null!;

    public virtual ICollection<Technic> Technics { get; set; } = new List<Technic>();
}
