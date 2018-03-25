using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningAspCoreWeb.Services;

namespace LearningAspCoreWeb.Controllers
{
    public class HomeController
    {
        readonly IHomeService _homeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="homeService">The home service.</param>
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// Indexes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task<int> Index(HttpContext context)
        {
            var sb = new StringBuilder();
            sb.Append("<h1>");
            sb.Append(_homeService.GetWelcomeMessages().First());
            sb.Append("</h1>");
            sb.Append("<hr>");
            sb.Append(string.Join("", _homeService.GetWelcomeMessages()
                .Where(m => !m.Equals(_homeService.GetWelcomeMessages().First()))
                .Select(m => $"<p>{m}</p>").ToArray()));

            await context.Response.WriteAsync(sb.ToString());
            return 200;
        }
    }
}