namespace Api;

using Application.Handlers;
using Application.Mapper;
using Application.Validators;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        builder.Services.AddControllers();

        builder.Services.AddDbContext<OracleContext>(opt =>
            opt.UseOracle(builder.Configuration.GetConnectionString("Oracle")));

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly));

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });

        builder.Services.AddValidatorsFromAssembly(typeof(RegisterUserValidator).Assembly);

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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

        var keyString = builder.Configuration.GetValue<string>("Jwt:Key");
        var key = Encoding.ASCII.GetBytes(keyString);
        var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
        var audience = builder.Configuration.GetValue<string>("Jwt:Audience");

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
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }

    public static void ConfigureMiddleWare(WebApplication app)
    {
            app.UseSwagger();
            app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors("AllowAll");

        app.UseAuthorization();

        app.MapControllers();
    }   
}
