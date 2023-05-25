using Framework;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository;
using System.Net;
using System.Security.Claims;

namespace Shop.API
{
    public class CustomAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userService = (IUserService)context.HttpContext.RequestServices.GetRequiredService(typeof(IUserService));
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            if (token.IsEmpty()) throw new UnauthorizedException(HttpStatusCode.Unauthorized, "TokenNotSend", "TokenIsEmpty");

            var actionName = context.ActionDescriptor.GetStringProperty("ActionName");
            var controllerName = context.ActionDescriptor.GetStringProperty("ControllerName");

            var permissionCode = $"{controllerName}/{actionName}";

            var login = Task.Run(() => userService.CheckToken(token, permissionCode)).GetAwaiter().GetResult();
            if (login != null)
            {
                bool IsFreeApi = false;
                if (controllerName == "Home" || actionName == "index")
                {
                    IsFreeApi = true;
                    token = "04386FD8-AA96-46BC-AF7A-FE0CE5CE37E1";
                }
                if (login == null && !IsFreeApi)
                    throw new UnauthorizedException(HttpStatusCode.Unauthorized, string.Empty, ConstProperty.ResponseMessage.InvalidToken);

                if (!IsFreeApi)
                    throw new UnauthorizedException(HttpStatusCode.Unauthorized, string.Empty, ConstProperty.ResponseMessage.AccessDenied);

                var userIdentity = new ClaimsIdentity("Identity");
                userIdentity.AddClaim(new Claim(ClaimTypes.Name, "public"));
                userIdentity.AddClaim(new Claim("Token", token));
                userIdentity.AddClaim(new Claim("UserId", login?.Id.ToString() ?? string.Empty));
                userIdentity.AddClaim(new Claim("RoleId", "public"));

                var identity = new ClaimsPrincipal(userIdentity);
                context.HttpContext.User = identity;
                base.OnActionExecuting(context);
            }
        }
    }
}