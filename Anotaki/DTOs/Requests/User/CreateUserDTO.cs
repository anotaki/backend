using System.ComponentModel.DataAnnotations;

namespace anotaki_api.DTOs.Requests.User
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(11)]
        public string Cpf { get; set; }
    }
}
