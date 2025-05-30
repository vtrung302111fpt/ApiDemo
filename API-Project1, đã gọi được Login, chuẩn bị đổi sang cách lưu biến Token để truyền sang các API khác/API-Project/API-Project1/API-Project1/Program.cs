﻿using API_Project1.Interfaces;
using API_Project1.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LoginConfig>(builder.Configuration.GetSection("Login"));
builder.Services.AddSingleton(resolver =>
{
    var config = resolver.GetRequiredService<IConfiguration>();
    var loginConfig = config.GetSection("Login").Get<LoginConfig>();
    return loginConfig;
});

//builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddHttpClient(); // Nếu chưa có
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>();
builder.Services.AddScoped<IInvoiceListService, InvoiceListService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
