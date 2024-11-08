using E_Commerce.Mailing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using E_Commerce.Mapper;
using System.Globalization;
using System.Text;
using Microsoft.OpenApi.Models;
using E_Commerce.Application.Interfaces;
using E_Commerce.Services;
using E_Commerce.Application.Helpers;
using E_Commerce.Data.Data;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure;
using E_Commerce.Application;

namespace E_Commerce
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<E_CommerceContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("E_Commerce"),
					builder => builder.EnableRetryOnFailure()));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<E_CommerceContext>()
				.AddDefaultTokenProviders();
			builder.Services.Configure<DataProtectionTokenProviderOptions>
	            (options => options.TokenLifespan = TimeSpan.FromHours(1));

			builder.Services.AddInfrastructureServices().AddReposetoriesServices();

			//[Authoriz] used JWT Token in Chck Authantiaction
			#region Authentication
			builder.Services.AddAuthentication(options =>
			  {
				  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				  options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
			  }
				  ).AddJwtBearer(o =>
				  {
					  o.IncludeErrorDetails = true;
					  o.RequireHttpsMetadata = false;
					  o.SaveToken = false;
					  o.TokenValidationParameters = new TokenValidationParameters
					  {
						  ValidateIssuerSigningKey = true,
						  ValidateAudience = true,
						  ValidateIssuer = true,
						  ValidateLifetime = true,
						  ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
						  ValidAudience = builder.Configuration["JWT:ValidAudience"],
						  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
					  };
				  });
			#endregion

			#region Mailing
			builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("Mailing"));
			builder.Services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);
			#endregion

			#region swagger
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trendoora E_Commerce", Version = "v1" });
			});
			builder.Services.AddSwaggerGen(swagger =>
			{
				//This?is?to?generate?the?Default?UI?of?Swagger?Documentation????
				swagger.SwaggerDoc("v2", new OpenApiInfo
				{
					Version = "v1",
					Title = "ASP.NET?5?Web?API",
					Description = " ITI Projrcy"
				});

				//?To?Enable?authorization?using?Swagger?(JWT)????
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter?'Bearer'?[space]?and?then?your?valid?token?in?the?text?input?below.\r\n\r\nExample:?\"Bearer?eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
				});
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
					new string[] {}
					}
				});
			});
			#endregion

			#region Configuration
			builder.Services.Configure<IdentityOptions>(options =>
				{
					// Password Settings
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequiredUniqueChars = 0;
					options.Password.RequiredLength = 2;
					options.Password.RequireNonAlphanumeric = false;

					//Lockout Settings
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
					options.Lockout.MaxFailedAccessAttempts = 5;
					options.Lockout.AllowedForNewUsers = true;

					//User Settings
					options.User.AllowedUserNameCharacters =
					 "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
					options.User.RequireUniqueEmail = true;

				});
			#endregion

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "T_ECOM"));
			}

			// Enable Request Localization Middleware
			//var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
			//app.UseRequestLocalization(localizationOptions);

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
