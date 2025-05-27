using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace anotaki_api.Models
{
    [Index(nameof(Cpf), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cpf { get; set; }
    }
}
