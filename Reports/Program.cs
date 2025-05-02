using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


// Получаем конфигурацию Keycloak
string? keycloakAuthority = builder.Configuration["Keycloak:Authority"];
string? keycloakAudience = builder.Configuration["Keycloak:Audience"];
string? keycloakIssuer = builder.Configuration["Keycloak:Issuer"];

// Добавляем аутентификацию
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakAuthority;
        options.Audience = keycloakAudience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = keycloakIssuer,
            ValidateLifetime = true
        };
        
        // Для разработки - отключаем HTTPS (только для тестов!)
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


WebApplication app = builder.Build();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
