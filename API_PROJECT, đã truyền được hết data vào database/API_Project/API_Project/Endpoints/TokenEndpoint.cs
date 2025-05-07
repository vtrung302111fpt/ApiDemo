using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace API_Project.Endpoints
{
    public static class TokenEndpoint
    {
        public static void MapTokenEndpoint(this WebApplication app)
        {
            app.MapPost("/api/token", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
            {
                var client = httpClientFactory.CreateClient();
                Dictionary<string, string>? requestBody = null;

                var config = app.Configuration;

                try
                {
                    requestBody = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);
                }
                catch
                {
                    context.Response.StatusCode = 400; // Bad request
                    await context.Response.WriteAsync("Request body format không hợp lệ");
                    return;
                }

                var username = requestBody?.GetValueOrDefault("username") ?? config["Login:Username"];
                var password = requestBody?.GetValueOrDefault("password") ?? config["Login:Password"];

                var apiBody = new
                {
                    username = username,
                    password = password
                };

                var content = new StringContent(JsonSerializer.Serialize(apiBody), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                context.Response.ContentType = "application/json";

                if (response.IsSuccessStatusCode)
                {
                    context.Response.StatusCode = (int)response.StatusCode;
                    await context.Response.WriteAsync(responseContent);
                }
                else
                {
                    context.Response.StatusCode = (int)response.StatusCode;
                    await context.Response.WriteAsync("Authentication không thành công");
                }
            });
        }
    }
}
