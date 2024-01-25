using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Migrations;
using TicacToe_Backend.Helpers.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPostgres(builder.Configuration)
                .AddMongo(builder.Configuration);

builder.Services.AddIdentityWithJWT();

builder.Services.AddMetdiator();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.SaveToken = true;   
    
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
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
            new string[] {}
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

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

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