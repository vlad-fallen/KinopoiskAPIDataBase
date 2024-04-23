using Asp.Versioning;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;


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

builder.Services.AddApiVersioning(options =>
    {
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
        


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "KinopoiskApi", Version = "v1.0" });
        options.SwaggerDoc("v2", new OpenApiInfo { Title = "KinopoiskApi", Version = "v2.0" });
    }
    /*opts =>
    opts.ResolveConflictingActions(apiDesc => apiDesc.First()) //решает конфликт с дублированием путей для апи \ не рекомендуется так делать*/
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Configuration.GetValue<bool>("UseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v1/swagger.json", $"KinopoiskApi v1");
        options.SwaggerEndpoint($"/swagger/v2/swagger.json", $"KinopoiskApi v2");
    });
}

if (app.Configuration.GetValue<bool>("UseDeveloperExcpeptionPage"))
    app.UseDeveloperExceptionPage();    // обработчик ошибок для development environment
else
    app.UseExceptionHandler("/error");  // обработчик ошибок для всех остальных окружений

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

// Minimal API
app.MapGet("/v{version:ApiVersion}/error",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => Results.Problem()); // возвращает страницу с ошибкой

app.MapGet("/v{version:ApiVersion}/error/test",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () =>
    { throw new Exception("test"); });

app.MapGet("/v{version:ApiVersion}/cod/test",
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [EnableCors("AnyOrigin_GetOnly")]
    [ResponseCache(NoStore = true)] () =>
    Results.Text("<script>" +
        "window.alert('Your client supports JavaScript!" +
        "\\r\\n\\r\\n" +
        $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
        "\\r\\n" +
        "Client time (UTC): ' + new Date().toISOString());" +
        "</script>" +
        "<noscript>Your client does not support JavaScript</noscript>",
        "text/html"));

//Controllers API
app.MapControllers().RequireCors("AnyOrigin");

app.Run();
