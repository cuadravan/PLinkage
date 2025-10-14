namespace PLinkageAPI.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ServiceResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ServiceResponse<T> { Success = true, Data = data, Message = message };
        }

        public static ServiceResponse<T> FailureResponse(string message)
        {
            return new ServiceResponse<T> { Success = false, Message = message };
        }
    }

    // Optional non-generic version
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public static ServiceResponse SuccessResponse(string? message = null)
            => new() { Success = true, Message = message };

        public static ServiceResponse FailureResponse(string message)
            => new() { Success = false, Message = message };
    }

}
