using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.Encodings.Web;

namespace LearningAspCoreWeb
{
    public static class RequestAndResponse
    {
        /// <summary>
        /// Sets up the input values for web page view.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDiv(string key, string value) =>
            $"<div><span>{key}:</span> <span>{value}</span></div>";

        /// <summary>
        /// Gets the request information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetRequestInformation(HttpRequest request)
        {
            var sb = new StringBuilder();
            sb.Append(GetDiv("scheme", request.Scheme));
            sb.Append(GetDiv("host", request.Host.HasValue ?
                request.Host.Value : "no host"));
            sb.Append(GetDiv("path", request.Path));
            sb.Append(GetDiv("query string", request.QueryString.HasValue ?
                request.QueryString.Value : "no query string"));
            sb.Append(GetDiv("method", request.Method));
            sb.Append(GetDiv("protocol", request.Protocol));

            return sb.ToString();
        }

        /// <summary>
        /// Gets the header information.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetHeaderInformation(HttpRequest request)
        {
            var sb = new StringBuilder();
            IHeaderDictionary headers = request.Headers;
            foreach (var header in request.Headers)
            {
                sb.Append(GetDiv(header.Key, string.Join("; ", header.Value)));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Performs an operation on the query string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string QueryString(HttpRequest request)
        {
            var sb = new StringBuilder();
            string xVal = request.Query["x"];
            string yVal = request.Query["y"];
            if (xVal == null || yVal == null)
            {
                return "x and y must both be set";
            }

            if (!int.TryParse(xVal, out int x))
            {
                sb.Append($"Error parsing {xVal}\n");
            }
            if (!int.TryParse(yVal, out int y))
            {
                sb.Append($"Error parsing {yVal}");
            }

            if (!sb.ToString().Equals(string.Empty))
            {
                return sb.ToString().Div();
            }
            return $"{x} + {y} = {x + y}".Div();
        }

        /// <summary>
        /// Produces the specified content on the page.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string Content(HttpRequest request) => request.Query["data"];

        /// <summary>
        /// Encodes the user-specified data to prevent page modification.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string ContentEncoded(HttpRequest request) =>
            HtmlEncoder.Default.Encode(request.Query["data"]);

        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetForm(HttpRequest request)
        {
            string result = string.Empty;
            switch (request.Method)
            {
                case "GET":
                    result = GetForm();
                    break;
                case "POST":
                    result = ShowForm(request);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <returns></returns>
        private static string GetForm() =>
            "<form method=\"post\" action=\"form\">" +
            "<input type=\"text\" name=\"text1\" />" +
            "<input type=\"submit\" value=\"Submit\" />";

        /// <summary>
        /// Shows the form.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private static string ShowForm(HttpRequest request)
        {
            var sb = new StringBuilder();
            if (request.HasFormContentType)
            {
                IFormCollection coll = request.Form;
                foreach (var key in coll.Keys)
                {
                    sb.Append(GetDiv(key, HtmlEncoder.Default.Encode(coll[key])));
                }
            }
            else
            {
                sb.Append("no form".Div());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Writes the cookie.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static string WriteCookie(HttpResponse response)
        {
            response.Cookies.Append("color", "red", new CookieOptions
            {
                Path = "/cookies",
                Expires = DateTime.Now.AddDays(1)
            });
            return "cookie written".Div();
        }

        /// <summary>
        /// Reads the cookie.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string ReadCookie(HttpRequest request)
        {
            var sb = new StringBuilder();
            IRequestCookieCollection cookies = request.Cookies;
            foreach (var key in cookies.Keys)
            {
                sb.Append(GetDiv(key, cookies[key]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static string GetJson(HttpResponse response)
        {
            var b = new
            {
                Title = "Tad: A Life - The Complete Autobiography of Tad McCorkle",
                Publisher = "The Publish Dudes",
                Author = "Tad McCorkle"
            };

            string json = JsonConvert.SerializeObject(b);
            response.ContentType = "application/json";
            return json;
        }
    }
}
