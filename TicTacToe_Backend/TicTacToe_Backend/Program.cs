using Microsoft.EntityFrameworkCore;
using Migrations;
using TicacToe_Backend.Helpers.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPostgres(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

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