using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventsAPI.Services
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute
    {
        private readonly string _role;
        public JwtAuthenticationAttribute(string role = "user")
        {
            _role = role;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token != null)
            {
                var claims = AuthenticationService.ValidateToken(token);

                if (!claims.IsInRole($"{_role}"))
                {
                    context.Result = new UnauthorizedObjectResult("No permission");
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult("token not found");
            }

            base.OnActionExecuted(context);
        }
    }
}
