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

app.MapTokenEndpoint();
app.MapGetUserInfo();
app.MapInvoiceList();

app.Run();



