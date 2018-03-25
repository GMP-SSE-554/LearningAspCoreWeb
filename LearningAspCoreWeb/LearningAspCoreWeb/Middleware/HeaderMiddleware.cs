using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LearningAspCoreWeb.Middleware
{
    public class HeaderMiddleware
    {
        readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware module.</param>
        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add(
                "initialheader", new string[] { "addheadermiddleware" });
            return _next(context);
        }
    }

    public static class HeaderMiddlewareExtensions
    {
        /// <summary>
        /// Uses the header middleware.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseHeaderMiddleware(
            this IApplicationBuilder builder) =>
            builder.UseMiddleware<HeaderMiddleware>();
    }
}
