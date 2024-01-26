using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Migrations;
using TicacToe_Backend.Helpers.Extensions;
using TicacToe_Backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMetdiator();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddJwtAuthorization(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "bearer",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement    {
        {
            new OpenApiSecurityScheme            {
                Reference = new OpenApiReference                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    var configuration = builder.Configuration;
    var front = configuration["FrontendHost:Front"]!;

    options.AddPolicy("DevelopPolicy",
        builder =>
            builder.WithOrigins(front)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true)
    );
} );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI()
        .UseCors("DevelopPolicy");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("game-hub");

await TryMigrateDatabaseAsync(app);
app.Run();

static async Task TryMigrateDatabaseAsync(WebApplication app)
{
    try
    {
        await using var scope = app.Services.CreateAsyncScope();
        var sp = scope.ServiceProvider;

        await using var db = sp.GetRequiredService<TicTacToeContext>();

        await db.Database.MigrateAsync();
    }
    catch (Exception e)
    {
        app.Logger.LogError(e, "Error while migrating the database");
        Environment.Exit(-1);
    }
}