using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Web.Common
{
    public static class RequestUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizeUserAuthorMiddleware>();
        }
    }

    public class AuthorizeUserAuthorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeUserAuthorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/api/question" && context.Request.Method == "POST")
            {

                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                var json = JsonConvert.DeserializeObject<dynamic>(body);

                Console.WriteLine($"STREAM {context.Request.Method}::{context.Request.Path}::{body}");

                json.AuthorId = context.User.GetSubjectIdentifier();
                body = JsonConvert.SerializeObject(json);

                Console.WriteLine($"STREAM new::{body}");

                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
            }

            await _next(context); // Call the next delegate/middleware in the pipeline
        }
    }
}