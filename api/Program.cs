using System.Reflection;
using api.app.db;
using api.app.stock.repository;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options => {
    //https://learn.microsoft.com/ko-kr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio-code#xml-comments
    // c# 설명 주석이 swagger에 반영됨
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    // https://learn.microsoft.com/ko-kr/aspnet/core/security/app-secrets
    // 시크릿 설정 => user-secrets init/add/list
    var conStrBuilder = new MySqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
    conStrBuilder.UserID = builder.Configuration["user"];
    conStrBuilder.Password = builder.Configuration["password"];
    string? connectionString = conStrBuilder.ConnectionString;
    options
    .UseMySql(connectionString, 
    ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IStockRepository, StockRepository>();

// DI 등 처리하는 메인 앱
var app = builder.Build();

// development일 때만 swagger 활성화
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
