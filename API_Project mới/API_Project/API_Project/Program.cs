//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using API_Project.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddSingleton<BillStoreService>();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.UseRouting();
//app.MapControllers();

//app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.BearerToken;
using API_Project.Models;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost")  // Đảm bảo rằng frontend của bạn đang chạy trên URL này
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});



// Đăng ký HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();


app.UseCors("AllowFrontend");

// Endpoint 1: Lấy Access Token
app.MapPost("/api/token", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{   var client = httpClientFactory.CreateClient();

    string? accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    string? refreshToken = context.Request.Headers["Authorization"].ToString();

    //if (IsAccessTokenExpired(accessToken))
    //{
    //    var refreshedTokens = await RefreshAccessToken(client, refreshToken);

    //    if (refreshedTokens != null)
    //    {
    //        accessToken = refreshedTokens.Value.AccessToken;
    //        refreshToken = refreshedTokens.Value.RefreshToken;

    //        context.Response.Headers.Add("New-Access-Token", accessToken);
    //        context.Response.Headers.Add("New-Refresh-Token", refreshToken);
    //    }
    //    else
    //    {
    //        context.Response.StatusCode = 401;
    //        await context.Response.WriteAsync("Đã hết phiên. Vui lòng đăng nhập lại");
    //        return;
    //    }
    //}

    Dictionary<string, string>? requestBody = null;


    var config = app.Configuration;

    try
    {
        requestBody = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);
    }
    catch
    {
        context.Response.StatusCode = 400; //Bad request
        await context.Response.WriteAsync("Request body format không hợp lệ");
        return;
    }


    //var requestBody = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);
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


//bool IsAccessTokenExpired(string accessToken)
//{
//    var tokenHandler = new JwtSecurityTokenHandler();
//    var token = tokenHandler.ReadJwtToken(accessToken);
//    var expriy = token.ValidTo;
//    return expriy < DateTime.UtcNow;
//}


//async Task<(string AccessToken, string RefreshToken)?> RefreshAccessToken(HttpClient client, string refreshToken)
//{
//    var refreshRequestBody = new
//    {
//        refresh_token = refreshToken
//    };

//    var refreshContent = new StringContent(JsonSerializer.Serialize(refreshRequestBody), Encoding.UTF8, "application/json");

//    var response = await client.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/refresh-token", refreshContent);

//    if (response.IsSuccessStatusCode)
//    {
//        var responseContent = await response.Content.ReadAsStringAsync();
//        var tokens = JsonSerializer.Deserialize<TokenResponse>(responseContent);

//        if (tokens != null)
//        {
//            return (tokens.AccessToken, tokens.RefreshToken);
//        }
//    }

//    return null; // Không thể refresh token, yêu cầu người dùng đăng nhập lại
//}


//Endpoint 2: Lấy User Info
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

//Endpoint 3: Lấy Invoice List
app.MapGet("/api/invoices", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();
    var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    var token = authorizationHeader?.Replace("Bearer ", "");

    var doanhnghiepma = context.Request.Headers["doanhnghiepma"].FirstOrDefault() ?? string.Empty;
    var userma = context.Request.Headers["userma"].FirstOrDefault() ?? string.Empty;

    var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");

    if (!string.IsNullOrEmpty(token))
    {
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    if (!string.IsNullOrEmpty(doanhnghiepma))
    {
        requestMessage.Headers.Add("doanhnghiepma", doanhnghiepma);
    }
    if (!string.IsNullOrEmpty(userma))
    {
        requestMessage.Headers.Add("userma", userma);
    }


    var response = await client.SendAsync(requestMessage);
    var responseContent = await response.Content.ReadAsStringAsync();

    context.Response.StatusCode = (int)response.StatusCode;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(responseContent);


    Console.WriteLine($"Token: {token}");
    Console.WriteLine($"DoanhNghiepMa: {doanhnghiepma}");
    Console.WriteLine($"UserMa: {userma}");

});

app.Run();
