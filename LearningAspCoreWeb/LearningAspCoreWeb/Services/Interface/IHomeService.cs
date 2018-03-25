using System.Collections.Generic;

namespace LearningAspCoreWeb.Services
{
    public interface IHomeService
    {
        /// <summary>
        /// Gets the welcome messages.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetWelcomeMessages();
    }
}
