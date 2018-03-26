
namespace LearningAspCoreWeb.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Divs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Div(this string value) =>
            $"<div>{value}</div>";

        /// <summary>
        /// Spans the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Span(this string value) =>
            $"<span>{value}</span>";
    }
}
