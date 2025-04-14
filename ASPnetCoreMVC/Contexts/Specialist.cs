using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class Specialist
{
    public int IdSpecialist { get; set; }

    public string LastNameSpecialist { get; set; } = null!;

    public string FirstNameSpecialist { get; set; } = null!;

    public string? PatronymicSpecialist { get; set; }

    public string? PhoneNumberSpecialist { get; set; }

    public string? EmailSpecialist { get; set; }

    public int IdUser { get; set; }

    public int IdPosition { get; set; }

    public virtual ICollection<HistoryOfChangingOrderStatus> HistoryOfChangingOrderStatuses { get; set; } = new List<HistoryOfChangingOrderStatus>();

    public virtual PositionOfASpecialist IdPositionNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
