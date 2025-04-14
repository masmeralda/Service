using System;
using System.Collections.Generic;
using ASPnetCoreMVC.Contexts;

namespace ASPnetCoreMVC.Models;

public class OrderViewModel
{
    public int IdOrder { get; set; }
    public string StatusOrder { get; set; } = null!;
    public DateTime? CreationDateOrder { get; set; }
    public DateTime? CompletionDateOrder { get; set; }
    public string? CommentOrder { get; set; }
    public int ClientId { get; set; }
    public ClientViewModel? Client { get; set; }
    public int SpecialistId { get; set; }
    public SpecialistViewModel? Specialist { get; set; }
    public int TechnicId { get; set; }
    public TechnicViewModel? Technic { get; set; }
    public int ServiceId { get; set; }
    public ServiceViewModel? Service { get; set; }

}

