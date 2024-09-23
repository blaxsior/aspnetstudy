using System.Reflection;
using api.app.comment.repository;
using api.app.db;
using api.app.stock.repository;
using api.app.token.service;
using api.app.user.entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    //https://learn.microsoft.com/ko-kr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio-code#xml-comments
    // c# 설명 주석이 swagger에 반영됨
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My Demo API",
        Version = "v1",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        // 헤더 이름
        Name = "Authorization",
        // Bearer token
        Scheme = "Bearer",
        // scheme의 위치
        In = ParameterLocation.Header,
        // 포맷 타입
        BearerFormat = "JWT",
        // http 기반
        Type = SecuritySchemeType.Http,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddControllers()
.AddNewtonsoftJson(options =>
{
    // efcore에서 발생하는 원형 참조 문제 방지
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
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
// 인증 옵션 설정
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
// efcore로 지원
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
    };
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

// DI 등 처리하는 메인 앱
var app = builder.Build();

// development일 때만 swagger 활성화
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 인증 설정
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
