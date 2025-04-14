using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class PositionOfASpecialist
{
    public int IdPosition { get; set; }

    public string NamePosition { get; set; } = null!;

    public virtual ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
}
