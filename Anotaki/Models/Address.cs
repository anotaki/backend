using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace anotaki_api.Models
{
    public class Address
    {
        public int Id { get; set; } 
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string? Complement { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
