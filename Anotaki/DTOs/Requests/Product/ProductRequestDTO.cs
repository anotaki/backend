using anotaki_api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anotaki_api.DTOs.Requests.Product
{
    public class ProductRequestDTO
    {
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
    }
}
