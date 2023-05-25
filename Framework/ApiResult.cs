using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Framework
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public int Count { get; set; }
        private ApiResult()
        {
            Message = string.Empty;
            Error = string.Empty;
        }

        public ApiResult(bool? isSuccess, string? message)
        {
            IsSuccess = isSuccess ?? false;
            Message = message ?? string.Empty;
            Error = string.Empty;
        }
        public ApiResult(bool isSuccess, string message, string error)
        {
            IsSuccess = isSuccess;
            Message = message;
            Error = error;
        }

        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true, " Success ");
        }
        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false, "Dto Not Valid .");
        }
    }

    public class ApiResult<TData> : ApiResult
    {
        public TData? Data { get; set; }
        public ApiResult(bool isSuccess, string message, TData? data) : base(isSuccess, message)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data ?? default;
        }
        public ApiResult(bool isSuccess, string message, string error, TData data) : base(isSuccess, message, error)
        {
            IsSuccess = isSuccess;
            Message = message;
            Error = error;
            Data = data;
        }
        public ApiResult(bool isSuccess, string message, string error, HttpStatusCode statusCode, int count, TData? data) : base(isSuccess, message, error)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message;
            Error = error;
            Count = count;
            Data = data;
        }

        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, " Success ", data);
        }

        public static implicit operator ApiResult<TData>(OkResult result)
        {
            return new ApiResult<TData>(true, " Success ", default);
        }

        public static implicit operator ApiResult<TData>(BadRequestResult result)
        {
            return new ApiResult<TData>(false, "Parameter Not Valid! ", default);
        }
    }
}