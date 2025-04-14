using ASPnetCoreMVC.Contexts;

namespace ASPnetCoreMVC.Models
{
    public class TechnicViewModel
    {
        public int IdTechnic { get; set; }
        public string? DescriptionTechnic { get; set; }
        public string? PhotoTechnic { get; set; }
        public int IdModelTechnic { get; set; }
        public ModelTechnicViewModel? ModelTechnic { get; set; } // Связь с моделью
    }

}
