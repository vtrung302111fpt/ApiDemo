using API_Project.Proxy;

namespace API_Project.Endpoints
{
    public static class TokenEndpoint
    {
        public static void MapTokenEndpoint(this WebApplication app)
        {
            app.MapPost("/api/token", async context =>
            {
                var factory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                await ProxyHelper.ProxyRequestPost(
                    context,
                    factory.CreateClient(),
                    "https://stg-accounts-api.xcyber.vn/management/cyberid/login"
                );
            });
        }
    }
}
