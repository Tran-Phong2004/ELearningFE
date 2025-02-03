using System.Text.Json.Serialization;

namespace ELearningFE.Models
{
    public class ApiResponse<T>
    {
        public int? code { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }

        // Đánh dấu constructor với JsonConstructor
        [JsonConstructor]
        public ApiResponse(int? code = null, T? data = default(T), string? message = null, bool success = false)
        {
            this.code = code;
            this.data = data;
            this.message = message;
            this.success = success;
        }
    }
}
