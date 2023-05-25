using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Shop.API
{
    public class CustomActionResult : IActionResult
    {
        private readonly BaseResponseDto _baseResponseDto;
        public CustomActionResult(BaseResponseDto baseResponseDto) => _baseResponseDto = baseResponseDto;

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_baseResponseDto)
            {
                StatusCode = _baseResponseDto.Status switch
                {
                    ResponseStatus.Success => (int)HttpStatusCode.OK,
                    ResponseStatus.NotFound => (int)HttpStatusCode.NotFound,
                    ResponseStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.BadRequest,
                }
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}