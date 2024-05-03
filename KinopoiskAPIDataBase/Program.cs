using EFDataAccessLibrary.DataAccess;
using KinopoiskAPIDataBase.Controllers;
using KinopoiskAPIDataBase.Swagger;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(cfg =>
        {
            cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
        options.AddPolicy(name: "AnyOrigin",
            cfg =>
            {
                cfg.AllowAnyOrigin();
                cfg.AllowAnyHeader();
                cfg.AllowAnyMethod();
            });
    });

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor( (x) => $"The value {x} is invalid.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => $"The field {x} is must be a number.");
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor( (x, y) => $"The value {x} is not valid for {y}.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "A value is required.");

}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ParameterFilter<SortColumnFilter>();
    options.ParameterFilter<SortOrderFilter>();
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Kinopoisk")));

//builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Configuration.GetValue<bool>("UseDeveloperExcpeptionPage"))
    app.UseDeveloperExceptionPage();    // обработчик ошибок для development environment
else
    app.UseExceptionHandler("/error");
    /*app.UseExceptionHandler(action =>
    {
        action.Run(async context =>
        {
            var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();

            var details = new ProblemDetails();

            details.Detail = exceptionHandler?.Error.Message;

            details.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id
                                            ?? context.TraceIdentifier;
            details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

            details.Status = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(details));
        });
    });*/  // обработчик ошибок для всех остальных окружений

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();


#region MinimalAPI

app.MapGet("/error", 
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    (HttpContext context) =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();

        var details = new ProblemDetails();

        details.Detail = exceptionHandler?.Error.Message;

        details.Extensions["traceId"] = System.Diagnostics.Activity.Current?.Id
                                        ?? context.TraceIdentifier;
        details.Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        
        details.Status = StatusCodes.Status500InternalServerError;
        
        return Results.Problem(details);
    }); // возвращает страницу с ошибкой

app.MapGet("/error/test", 
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => { throw new Exception("test"); });

app.MapGet("/cod/test",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] 
    () =>  Results.Text("<script>" +
        "window.alert('Your client supports JavaScript!" +
        "\\r\\n\\r\\n" +
        $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
        "\\r\\n" +
        "Client time (UTC): ' + new Date().toISOString());" +
        "</script>" +
        "<noscript>Your client does not support JavaScript</noscript>",
        "text/html"));

#endregion


//Controllers API
app.MapControllers().RequireCors("AnyOrigin");

app.Run();
