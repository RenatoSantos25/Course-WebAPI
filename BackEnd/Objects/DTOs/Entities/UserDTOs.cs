using System.ComponentModel.DataAnnotations;

namespace ReserveiAPI.Objects.DTOs.Entities
{
    public class UserDTOs
    {
        public int Id { get; set; }

        public string? ImageProfile { get; set; }

        [Required(ErrorMessage = "O nome é requerido!")]
        [MaxLength(100)]

        public string NameUSer { get; set; }

        [Required(ErrorMessage = "o email é requerido !")]
        [MaxLength(200)]
        [MinLength(10)]
        private string _emailUser;

        public string EmailUser     
        {
            get => _emailUser;
            set => _emailUser = value.ToLower();

        }
        [Required(ErrorMessage = "A senha é requerido !")]
        [MinLength(8)]
        public string PasswordUser { get; set; }

        [Required(ErrorMessage = "Telefone é requerido !")]
        [MaxLength(15)]
        [MinLength(14)]
        public string PhoneUser { get; set; }
    }
}
