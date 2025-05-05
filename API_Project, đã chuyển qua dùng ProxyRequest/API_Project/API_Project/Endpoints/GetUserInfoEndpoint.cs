using API_Project.Proxy;

namespace API_Project.Endpoints
{
    public static class GetUserInfoEndpoint
    {
        public static void MapGetUserInfo(this WebApplication app) 
        {
            app.MapGet("/api/get-user-info", async context =>
            {
                var factory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                await ProxyHelper.GetProxyRequest(context, factory.CreateClient(), "https://dev-billstore.xcyber.vn/api/hddv/nguoidung/get-user-info");
            });
        }
    }
}
