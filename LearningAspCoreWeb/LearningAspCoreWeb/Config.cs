using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace LearningAspCoreWeb
{
    public static class Config
    {
        /// <summary>
        /// Reads the app settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static async Task AppSettings(HttpContext context, IConfigurationRoot config)
        {
            string settings = config["AppSettings:SiteName"];
            await context.Response.WriteAsync(settings.Div());
        }

        /// <summary>
        /// Reads the database connection.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static async Task ReadDatabaseConnection(HttpContext context, IConfigurationRoot config)
        {
            string connectionString = config["ConnectionStrings:DefaultConnection"];
            await context.Response.WriteAsync(connectionString.Div());
        }

        /// <summary>
        /// Reads the secret.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static async Task ReadSecret(HttpContext context, IConfigurationRoot config)
        {
            string secretString = config["Secret"];
            await context.Response.WriteAsync(secretString.Div());
        }
    }
}
