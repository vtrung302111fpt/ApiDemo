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

var builder = WebApplication.CreateBuilder(args);

// Đăng ký HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

// Endpoint 1: Lấy Access Token
app.MapPost("/api/token", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{   var client = httpClientFactory.CreateClient();

    Dictionary<string, string>? requestBody = null;


    var config = app.Configuration;
    //var requestBody = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);
    var username = requestBody?.GetValueOrDefault("username") ?? config["Login:Username"];
    var password = requestBody?.GetValueOrDefault("password") ?? config["Login:Password"];

    

    try
    {
        requestBody = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);
    }
    catch
    {

    }

    var apiBody = new
    {
        username = username,
        password = password
    };

    var content = new StringContent(JsonSerializer.Serialize(apiBody), Encoding.UTF8, "application/json");

    var response = await client.PostAsync("https://stg-accounts-api.xcyber.vn/management/cyberid/login", content);
    var responseContent = await response.Content.ReadAsStringAsync();

    context.Response.StatusCode = (int)response.StatusCode;
    await context.Response.WriteAsync(responseContent);
});

// Endpoint 2: Lấy Invoice List
app.MapGet("/api/invoices", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    var token = context.Request.Headers["token"].FirstOrDefault();
    var doanhnghiepma = context.Request.Headers["doanhnghiepma"].FirstOrDefault() ?? "";
    var userma = context.Request.Headers["userma"].FirstOrDefault() ?? "";

    var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://dev-billstore.xcyber.vn/api/hddv-hoa-don/get-list?current=1&page=0&pageSize=10&size=10&trangThaiPheDuyet");
    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    requestMessage.Headers.Add("doanhnghiepma", doanhnghiepma);
    requestMessage.Headers.Add("userma", userma);

    var response = await client.SendAsync(requestMessage);
    var responseContent = await response.Content.ReadAsStringAsync();

    context.Response.StatusCode = (int)response.StatusCode;
    await context.Response.WriteAsync(responseContent);
});

app.Run();
