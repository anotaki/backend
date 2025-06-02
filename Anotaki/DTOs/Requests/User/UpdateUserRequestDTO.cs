using System.ComponentModel.DataAnnotations;

namespace anotaki_api.DTOs.Requests.User
{
    public class UpdateUserRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
