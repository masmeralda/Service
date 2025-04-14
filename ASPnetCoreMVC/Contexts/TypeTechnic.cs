using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class TypeTechnic
{
    public int IdTypeTechnic { get; set; }

    public string NameTypeTechnic { get; set; } = null!;

    public virtual ICollection<ModelTechnic> ModelTechnics { get; set; } = new List<ModelTechnic>();
}
