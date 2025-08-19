namespace Api;

using Application.Handlers;
using Application.Validators;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation;
using Application.Mapper;
using Microsoft.Extensions.DependencyInjection;

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

        builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

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
