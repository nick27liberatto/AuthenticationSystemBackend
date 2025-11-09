namespace Api;

using Application.Handlers;
using Application.Interfaces;
using Application.Mapper;
using Application.Validators;
using Domain.Models;
using FluentValidation;
using Infrastructure.Authentication;
using Infrastructure.Configurations;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public class InitializerExtension
{
    public static WebApplication Initialize(WebApplicationBuilder builder)
    {
        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureMiddleWare(app);

        return app;
    }

    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly));

        builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserValidator).Assembly);

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AuthProfile>();
        });


        builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("Frontend"));
        builder.Services.AddSingleton<IFrontendSettings>(sp =>
            sp.GetRequiredService<IOptions<FrontendSettings>>().Value
        );

        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));
        builder.Services.AddTransient<IEmailService, SmtpEmailService>();

        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequiredLength = 10;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API",
                Version = "v1",
                Description = "API built with Clean Architecture, CQRS and .NET 6",
                Contact = new OpenApiContact
                {
                    Name = "Nicolas Liberatto",
                    Url = new Uri("https://github.com/nick27liberatto")
                }
            });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
    }

    public static void ConfigureMiddleWare(WebApplication app)
    {
            app.UseSwagger();
            app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowAll");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
    }   
}
