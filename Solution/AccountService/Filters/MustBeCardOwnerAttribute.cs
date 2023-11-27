using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace AccountService.Filters
{
    public class MustBeCardOwnerAttribute: TypeFilterAttribute
    {
        public MustBeCardOwnerAttribute() : base(typeof(MustBeCardOwnerFilter))
        {
        }
    }

    public class MustBeCardOwnerFilter : IAuthorizationFilter
    {
        private readonly ICardService _cardService;

        public MustBeCardOwnerFilter(ICardService cardService)
        {
            _cardService = cardService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var cardId = context.HttpContext.Request.RouteValues["id"]?.ToString();

            if (!_cardService.IsUserCardOwner(int.Parse(userId), int.Parse(cardId)))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
