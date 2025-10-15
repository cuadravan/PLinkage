namespace PLinkageShared.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } 

        public static ApiResponse<T> Ok(T data, string message = "Success")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = default };
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public static ApiResponse SuccessResponse(string? message = null)
            => new() { Success = true, Message = message };

        public static ApiResponse FailResponse(string message)
            => new() { Success = false, Message = message };
    }

}
