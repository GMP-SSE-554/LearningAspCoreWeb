using System.Collections.Generic;

namespace LearningAspCoreWeb.Services
{
    public class HomeService : IHomeService
    {
        /// <summary>
        /// The welcome messages
        /// </summary>
        List<string> welcomeMessages = new List<string>
        {
            "Welcome to this site!",
            "There really isn't much here...",
            "Come back later!"
        };

        /// <summary>
        /// Gets the welcome messages.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetWelcomeMessages() => welcomeMessages;
    }
}
