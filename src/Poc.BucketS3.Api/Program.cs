using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Poc.BucketS3.Api;
using Poc.BucketS3.Api.Middlewares;
using Poc.BucketS3.Domain.Commands;
using Serilog;
using Serilog.Events;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "Information";
        var serilogLogLevel = GetLogLevel(logLevel);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(serilogLogLevel)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();

        try
        {
            Log.Information("Starting application");

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services
                .AddMvc()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteFileCommand).Assembly));

            builder.Services.AddDependencyInjection();

            builder.Services.AddAwsBucketS3();

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;

                options.DefaultApiVersion = new ApiVersion(1, 0);

                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<CorrelationIdMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application failed to start");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    ///     Maps the string representation of the log level to the Serilog log level.
    /// </summary>
    /// <param name="logLevel">String representation of the log level.</param>
    /// <returns>Mapped Serilog log level.</returns>
    private static LogEventLevel GetLogLevel(string logLevel)
    {
        return logLevel.ToLower() switch
        {
            "verbose" => LogEventLevel.Verbose,
            "debug" => LogEventLevel.Debug,
            "information" => LogEventLevel.Information,
            "warning" => LogEventLevel.Warning,
            "error" => LogEventLevel.Error,
            "fatal" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information
        };
    }
}