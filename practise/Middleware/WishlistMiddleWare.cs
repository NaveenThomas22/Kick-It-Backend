using System.Security.Claims;

namespace practise.Middleware
{
    public class WishlistMiddleWare
    {
        private readonly RequestDelegate _next;

        public WishlistMiddleWare (RequestDelegate next)
        {
            _next = next;
        }

        public async Task  InvokeAsync (HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if(idClaim != null)
                {
                    context.Items["userid"] = idClaim.Value;

                 }

            }
            await _next(context);
        }
    }
}