using EFDataAccessLibrary.DataAccess;
using KinopoiskAPIDataBase.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(/*opts =>
    opts.ResolveConflictingActions(apiDesc => apiDesc.First()) //решает конфликт с дублированием путей для апи \ не рекомендуется так делать
    */);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Kinopoisk")));

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
    app.UseExceptionHandler("/error");  // обработчик ошибок для всех остальных окружений

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

// Minimal API
app.MapGet("/error", 
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => Results.Problem()); // возвращает страницу с ошибкой
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

//Controllers API
app.MapControllers().RequireCors("AnyOrigin");

app.Run();
