using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class User
{
    public int IdUser { get; set; }

    public string LoginUser { get; set; } = null!;

    public string PasswordUser { get; set; } = null!;

    public int? IdRole { get; set; }

    public virtual RoleUser? IdRoleNavigation { get; set; }

    public virtual ICollection<Specialist> Specialists { get; set; } = new List<Specialist>();
}
