using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace AccountService.Filters
{
    public class MustBeAccountOwnerAttribute: TypeFilterAttribute
    {
        public MustBeAccountOwnerAttribute() : base(typeof(MustBeAccountOwnerFilter))
        {
        }
    }

    public class MustBeAccountOwnerFilter : IAuthorizationFilter
    {
        private readonly IUserService _userService;

        public MustBeAccountOwnerFilter(IUserService orderService)
        {
            _userService = orderService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var requestUserId = context.HttpContext.Request.RouteValues["id"]?.ToString();

            if (userId != requestUserId)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
