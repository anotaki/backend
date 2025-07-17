using System.ComponentModel.DataAnnotations;

namespace anotaki_api.DTOs.Requests.Api
{
    public class PaginationParams
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
