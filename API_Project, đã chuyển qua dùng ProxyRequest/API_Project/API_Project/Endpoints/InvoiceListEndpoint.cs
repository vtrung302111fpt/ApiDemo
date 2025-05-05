using API_Project.Proxy;

namespace API_Project.Endpoints
{
    public static class InvoiceListEndpoint
    {
        public static void MapInvoiceList(this WebApplication app)
        {
            app.MapGet("/api/invoices", async context =>
            {
                var doanhnghiepma = context.Request.Headers["doanhnghiepma"].FirstOrDefault() ?? string.Empty;
                var userma = context.Request.Headers["userma"].FirstOrDefault() ?? string.Empty;

                var factory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                await ProxyHelper.GetProxyRequest(context, factory.CreateClient(),
                "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet",
                new Dictionary<string, string>
                {
                    { "doanhnghiepma", doanhnghiepma },
                    { "userma", userma }
                });
            });
        }
    }
}
