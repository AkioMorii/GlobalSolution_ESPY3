using System;

namespace GS2_APP.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Error { get; set; }
        public string? status { get; set; }
        public T? Data { get; set; }
    }
}
