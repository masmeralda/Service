using ASPnetCoreMVC.Contexts;

namespace ASPnetCoreMVC.Models
{
    public class ModelTechnicViewModel
    {
        public int IdModelTechnic { get; set; }
        public string? NameModelTechnic { get; set; }
        public int IdTypeTechnic { get; set; }
        public TypeTechnicViewModel? TypeTechnic { get; set; } // Связь с типом техники
    }
}
