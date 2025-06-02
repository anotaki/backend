namespace anotaki_api.Models.Response
{
    public class ApiResponse<T>
    {
        public string Title { get; set; }
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ApiResponse(string title, T? data = default)
        {
            Title = title;
            Data = data;
        }

    }
}
