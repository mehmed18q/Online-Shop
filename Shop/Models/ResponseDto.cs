namespace Shop.API
{
    public class BaseResponseDto
    {
        public ResponseStatus Status { get; set; } = ResponseStatus.Success;
        public string? Message { get; set; }
        public string? Errors { get; set; }
        public object? Data { get; set; }

        public static BaseResponseDto Success(string? message = null, string? errors = null)
        {
            return new BaseResponseDto
            {
                Status = ResponseStatus.Success,
                Message = message ?? errors,
            };
        }

        public static BaseResponseDto Fail(string? message = null, string? errors = null)
        {
            return new BaseResponseDto
            {
                Status = ResponseStatus.Fail,
                Message = message,
                Errors = errors
            };
        }
    }

    public class ListResponseDto : BaseResponseDto
    {
        public int Count { get; set; }
        public static ListResponseDto Success(string? message = null) => new() { Status = ResponseStatus.Success, Message = message };
    }

    public enum ResponseStatus
    {
        Unknown,
        Fail,
        Success,
        NotValid,
        NotFound,
        Unauthorized
    }

    public class ConstProperty
    {
        public static class ResponseMessage
        {
            public const string AddSuccess = "ثبت شد.";
            public const string InternalServerError = "خطایی رخ داده!";
            public const string CanNotDelete = "امکان حذف وجود ندارد.";
            public const string EditSuccess = "ویرایش شد.";
            public const string DeleteSuccess = "حذف شد.";
            public const string UploadFail = "آپلود انجام نشد.";
            public const string Unauthorized = "دسترسی ندارید.";
            public const string LoginFail = "نام‌کاربری یا رمز عبور اشتباه است.";
            public const string TableNameIsNotAllowed = "نام جدول غیرمجاز است.";
            public const string InvalidToken = "توکن معتبر نیست.";
            public const string AccessDenied = "دسترسی ندارید.";
        }
    }
}