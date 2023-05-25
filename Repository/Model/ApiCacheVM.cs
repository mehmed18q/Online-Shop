namespace Repository
{
    public class ApiCacheVM
    {
        public string? Type { get; set; }
        public string? Timer { get; set; }
        public string? Api { get; set; }
        public string? Json { get; set; }
        public string? Result { get; set; }

    }

    public class GetApiResultVM
    {
        public bool? IsSuccess { get; set; }
        public string? Message { get; set; }
        public byte[]? Data { get; set; }

    }
}