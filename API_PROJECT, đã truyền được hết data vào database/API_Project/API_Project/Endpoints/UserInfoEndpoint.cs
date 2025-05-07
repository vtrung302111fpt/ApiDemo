using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace API_Project.Endpoints
{
    public static class UserInfoEndpoint
    {
        public static void MapUserInfoEndpoint(this WebApplication app)
        {
            app.MapGet("/api/get-user-info", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
            {
                var client = httpClientFactory.CreateClient();
                var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                var token = authorizationHeader?.Replace("Bearer ", "");

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");
                if (!string.IsNullOrEmpty(token))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                context.Response.StatusCode = (int)response.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(responseContent);
            });
        }
    }
}
