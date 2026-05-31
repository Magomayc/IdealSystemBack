using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Gado.Api.Middlewares
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public SwaggerBasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                var username = configuration["SwaggerSettings:Username"] ?? "admin";
                var password = configuration["SwaggerSettings:Password"] ?? "admin123";

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    await _next(context);
                    return;
                }

                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var headerVal = AuthenticationHeaderValue.Parse(authHeader);
                        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(headerVal.Parameter)).Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            var user = credentials[0];
                            var pass = credentials[1];

                            if (user == username && pass == password)
                            {
                                await _next(context);
                                return;
                            }
                        }
                    }
                    catch
                    {
                        // Se falhar na decodificação ou validação, cairá no 401 abaixo
                    }
                }

                // Solicita autenticação básica do navegador
                context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger UI\"";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
