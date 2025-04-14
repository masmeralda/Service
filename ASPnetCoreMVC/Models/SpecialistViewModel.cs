namespace ASPnetCoreMVC.Models
{
    public class SpecialistViewModel
    {
        public int IdSpecialist { get; set; }
        public string LastNameSpecialist { get; set; } = null!;
        public string FirstNameSpecialist { get; set; } = null!;
        public string? PatronymicSpecialist { get; set; }
        public string? PhoneNumberSpecialist { get; set; }

        public PositionOfASpecialistViewModel? NamePosition { get; set; } // Связь с моделью должность специалиста
        public UserViewModel? LoginUser { get; set; } // Связь с моделью пользователь

    }
}
