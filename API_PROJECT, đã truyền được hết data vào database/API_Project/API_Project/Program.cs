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
using API_Project.Endpoints;

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

app.UseCors("AllowFrontend");

app.MapTokenEndpoint();
app.MapUserInfoEndpoint();
app.MapInvoiceListEndpoint();

app.Run();