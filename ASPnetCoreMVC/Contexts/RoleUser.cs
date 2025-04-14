using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class RoleUser
{
    public int IdRole { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
