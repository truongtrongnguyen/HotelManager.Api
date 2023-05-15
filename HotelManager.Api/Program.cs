using HotelManage.Authentication.Configuration;
using HotelManager.DataService.Data;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    var connect = builder.Configuration.GetConnectionString("AppDbContext");
    option.UseSqlServer(connect);
});

// Add UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Get key jwt
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// Add Jwt
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]);

var tokenValidationParameter = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireAudience = false,
    ValidateLifetime = true
};

builder.Services.AddSingleton(tokenValidationParameter);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;

    jwt.TokenValidationParameters = tokenValidationParameter;
});

// Microsoft.AspNetCore.Identity.UI
builder.Services.AddDefaultIdentity<AppUser>(option =>
    option.SignIn.RequireConfirmedAccount = true
).AddEntityFrameworkStores<AppDbContext>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CẤU HÌNH ĐƯỜNG DẪN LƯU FILE 
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/Uploads"
});


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
