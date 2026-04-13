using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Project.src.Domain.Interfaces;
using Project.src.Domain.Services;
using Project.src.DTOs;
using Project.src.Infra.Data;
using Project.src.Infra.Repository;
using Project.src.Infra.Repository.Interfaces;
using Project.src.Middlewares;
using Project.src.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Argon2Settings>(builder.Configuration.GetSection("Argon2Config"));

builder.Services.AddCors( options => {
    options.AddPolicy("AllowHost", policy => 
        policy.WithOrigins("http://localhost:5000").AllowAnyHeader().AllowAnyMethod()
    );
});

var connection_string = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(connection_string));

string key_string = builder.Configuration["JWT:Key"] 
    ?? throw new Exception("JWT:Key não configurado");
var key = Encoding.ASCII.GetBytes(key_string);

string issuer = builder.Configuration["JWT:Issuer"] 
    ?? throw new Exception("JWT:Issuer não configurado");

string audience = builder.Configuration["JWT:Audience"] 
    ?? throw new Exception("JWT:Audience não configurado");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ValidateIssuer = true,
        ValidIssuer = issuer,

        ValidateAudience = false,
        ValidAudience = audience,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Append("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", doc),
            new List<string>()
        } 
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("admin"));
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MapGet("/admin_generator", async ([FromServices] JwtService jwtService) =>
    {
        var token = jwtService.GenerateToken(new UserDTO(
            Guid.NewGuid(),
            "Rosvaldo",
            "EmailSuperSecre@gmail.com",
            "admin"
        ));

        return Results.Ok(new {token});
    }).WithTags("Admin");
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowHost");

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UserRoutes();
app.ProductRoutes();

app.Run();
