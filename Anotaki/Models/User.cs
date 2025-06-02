using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Models
{
	[Index(nameof(Cpf), IsUnique = true)]
	[Index(nameof(Email), IsUnique = true)]
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public Role Role { get; set; } = Role.Default;

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(6, ErrorMessage = "Password must be at least 4 characters.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "CPF is required.")]
		[RegularExpression(@"^\d{11}$", ErrorMessage = "CPF must be exactly 11 numeric digits.")]
		public string Cpf { get; set; }

		public List<Address> Addresses { get; set; } = [];

		public bool IsActive { get; set; } = true;
    }

	public static class Roles
	{
		public const string Admin = "admin";
		public const string Default = "default";
	}

	public enum Role
	{
		Default,
		Admin,
	}
}
