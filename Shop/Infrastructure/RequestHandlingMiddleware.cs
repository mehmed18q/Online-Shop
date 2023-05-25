using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Net;

namespace Shop.API
{
    // ToDo: chenge Return Response Text
    public class RequestHandlingMiddleware
    {
        private readonly Stopwatch _stopwatch;
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestHandlingMiddleware> _logger;

        public RequestHandlingMiddleware(RequestDelegate next, ILogger<RequestHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            var stream = context.Response.Body;
            var responseBuffer = new MemoryStream();
            try
            {
                _stopwatch.Restart();
                await _next(context);
                _stopwatch.Stop();

                var time = _stopwatch.ElapsedMilliseconds;
                if (time > 300)
                {
                    var api = $"({request.Method}) Core{request.Path}";
                    var token = request.Headers["Authorization"];
                    _logger.LogError(message: $"Time: {time},API: {api},RequestContent,Token: {token}");
                }

                responseBuffer.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(responseBuffer).ReadToEnd();
            }
            catch (Exception ex)
            {
                var headers = JsonConvert.SerializeObject(context.Request.Headers.Select(g => new { key = g.Key, value = g.Value }).ToList());
                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                responseBuffer.Seek(0, SeekOrigin.Begin);
                await responseBuffer.CopyToAsync(stream);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string error = ConstProperty.ResponseMessage.InternalServerError;
            string internalError = ex.Message.IsEmpty() ? ex.GetStringProperty("ResourceKey") : ex.Message;
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            var responseStatus = ResponseStatus.Fail;

            if (ex is CanNotDeleteException)
            {
                code = HttpStatusCode.BadRequest;
                error = ConstProperty.ResponseMessage.CanNotDelete;
            }
            else if (ex is UnauthorizedException)
            {
                code = HttpStatusCode.Unauthorized;
                responseStatus = ResponseStatus.Unauthorized;
                error = ConstProperty.ResponseMessage.InternalServerError;
            }

            var baseResponse = new BaseResponseDto
            {
                Status = responseStatus,
                Message = internalError,
                Errors = error
            };
            var result = JsonConvert.SerializeObject(baseResponse,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }

    public static class RequestHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<RequestHandlingMiddleware>();
    }
}