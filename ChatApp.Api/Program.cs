using ChatApp.Api;
using ChatApp.Api.Hubs;
using ChatApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// — ваши сервисы —
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddAuthenticationServices(builder.Configuration)
    .AddSwaggerServices()
    .AddLoggingServices()
    .AddMappingServiceCollection();

var app = builder.Build();

// 1) (опционально) HTTPS редирект
// app.UseHttpsRedirection();

// 2) Статические файлы и Swagger
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();

// 3) Роутинг + аутентификация/авторизация
app.UseRouting();              // <- Обязательно!
app.UseAuthentication();
app.UseAuthorization();

// 4) Ваш кастомный middleware (он сможет видеть уже авторизованного пользователя)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 5) Эндпоинты
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // Здесь остаётся ваш «абсолютный» путь:
    endpoints.MapHub<ChatHub>("/hubs/chat");
});

app.Run();