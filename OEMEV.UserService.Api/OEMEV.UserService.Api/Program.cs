using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Application.Services;
using OEMEV.UserService.Infrastructure.Base;
using OEMEV.UserService.Infrastructure.DBContext;
using OEMEV.UserService.Infrastructure.Interfaces;
using OEMEV.UserService.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngular", policy =>
	{
		policy.WithOrigins("http://localhost:5055")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});
#endregion

#region Database Context
var connString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseNpgsql(dataSource)
		   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
#endregion

builder.Services.AddControllers();

builder.Services.AddOpenApi();

#region JWT Authentication
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTSettings>()
	?? throw new InvalidOperationException("JWT Settings are not configured.");
jwtSettings.IsValid();

var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey!);
#endregion

#region Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = builder.Environment.IsProduction();
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = true,
		ValidIssuer = jwtSettings.Issuer,
		ValidateAudience = true,
		ValidAudience = jwtSettings.Audience,
		ClockSkew = TimeSpan.Zero
	};
});
#endregion

#region Dependency Injection (DI)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceCenterRepository, ServiceCenterRepository>();
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IServiceProviders, ServiceProviders>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IServiceCenterService, ServiceCenterService>();
builder.Services.AddScoped<IManufactureService, ManufactureService>();
builder.Services.AddScoped<IRoleService, RoleService>();
#endregion

#region Swagger Setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "OEMEV API",
		Version = "v1",
		Description = "OEMEV Backend API with JWT authentication"
	});

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});
#endregion

builder.Services.AddSwaggerGen(c =>
{
	c.EnableAnnotations();
});

var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "OEMEV UserService API v1");
	c.RoutePrefix = string.Empty;
});
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();