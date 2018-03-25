using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LearningAspCoreWeb.Middleware
{
    public class HeadingOneMiddleware
    {
        readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeadingOneMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public HeadingOneMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("<h1>HeadingOneMiddleware</h1>");
            await _next(context);
        }
    }

    public static class HeadingMiddlewareExtensions
    {
        /// <summary>
        /// Uses the heading one middleware.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseHeadingOneMiddleware(
            this IApplicationBuilder builder) =>
            builder.UseMiddleware<HeadingOneMiddleware>();
    }
}
