using System.Text;
using System.Net.Http.Headers;

namespace API_Project.Proxy;
public static class ProxyHelper
{
    public static async Task ProxyRequestPost(HttpContext context, HttpClient client, string targetUrl)
    {
        // Đọc body từ request của frontend
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, targetUrl)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };

        var response = await client.SendAsync(requestMessage);
        var responseContent = await response.Content.ReadAsStringAsync();

        context.Response.StatusCode = (int)response.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(responseContent);
    }




    public static async Task GetProxyRequest(
        HttpContext context, 
        HttpClient client, 
        string targetUrl,
        //Func<HttpRequestMessage, Task?> onSuccess =null,
        Dictionary<string, string>? extraHeaders = null
     )
    {


        //var requestMessage = new HttpRequestMessage
        //{
        //    Method = new HttpMethod(context.Request.Method),
        //    RequestUri = new Uri(targetUrl)
        //};

        //// Nếu có body (POST, PUT)
        //if (context.Request.ContentLength > 0)
        //{
        //    requestMessage.Content = new StreamContent(context.Request.Body);
        //}

        //var response = await client.SendAsync(requestMessage);

        //// Nếu bạn muốn xử lý nội dung JSON trước khi gửi về client:
        //if (response.IsSuccessStatusCode && onSuccess != null)
        //{
        //    await onSuccess(response);
        //}

        //// Forward lại response cho client
        //context.Response.StatusCode = (int)response.StatusCode;
        //context.Response.ContentType = response.Content.Headers.ContentType?.ToString();

        //await response.Content.CopyToAsync(context.Response.Body);



        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authorizationHeader?.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Không tìm thấy token trong Authorization header");
            return;
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, targetUrl);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (extraHeaders != null)
        {
            foreach (var header in extraHeaders)
            {
                if (!string.IsNullOrEmpty(header.Value))
                    requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await client.SendAsync(requestMessage);
        var responseContent = await response.Content.ReadAsStringAsync();

        context.Response.StatusCode = (int)response.StatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(responseContent);
    }

}
