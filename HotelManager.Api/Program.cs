using HotelManage.Authentication.Configuration;
using HotelManager.DataService.Data;
using HotelManager.DataService.IConfiguration;
using HotelManager.Entities.DbSet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]);

    jwt.SaveToken = true;

    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer  = false,
        ValidateAudience = false,
        RequireAudience =false,
        ValidateLifetime = true
    };
});

// Microsoft.AspNetCore.Identity.UI
builder.Services.AddDefaultIdentity<AppUser>(option =>
    option.SignIn.RequireConfirmedAccount = true
).AddEntityFrameworkStores<AppDbContext>();



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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
