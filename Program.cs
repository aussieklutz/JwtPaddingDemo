using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

// Configure the Authentication

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidIssuer = "test",
        ValidAudience = "test",
        RequireSignedTokens = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("shortkey")),
        ValidateLifetime = false
    };
});

builder.Services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticatedAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

var app = builder.Build();

// Enable Auth
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Enable Cors
// app.UseCors(CorsPolicy);

app.MapControllers();

app.RunAsync("https://localhost:50443");

var builder2 = WebApplication.CreateBuilder(args);

// Configure the Authentication

builder2.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidIssuer = "test",
        ValidAudience = "test",
        RequireSignedTokens = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("shortkey".PadRight((256 / 8), '\0'))),
        ValidateLifetime = false
    };
});

builder2.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});
// Add services to the container.

builder2.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder2.Services.AddEndpointsApiExplorer();
builder2.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticatedAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

var app2 = builder2.Build();

// Enable Auth
app2.UseAuthentication();
app2.UseAuthorization();

// Configure the HTTP request pipeline.
if (app2.Environment.IsDevelopment())
{
    app2.UseSwagger();
    app2.UseSwaggerUI();
}

//app2.UseHttpsRedirection();

// Enable Cors
// app.UseCors(CorsPolicy);

app2.MapControllers();

app2.RunAsync("https://localhost:51443");

Thread.Sleep(5000);

string bearer = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.ISl-kYtYCmmD4Er8oXOlyDz3PHsmZDsdd1AmMaKtLCA";

try
{
    HttpClient client = new();
    client.DefaultRequestHeaders.Add("Authorization", bearer);
    client.BaseAddress = new Uri("https://localhost:50443");
    Task<string> httpTask1 = client.GetStringAsync("/User");
    string resultWithoutPadding = httpTask1.Result;
    System.Console.WriteLine("The app instance without the padding returned the following:");
    System.Console.WriteLine(resultWithoutPadding);
}
catch (AggregateException ex)
{
    System.Console.WriteLine("The app instance without the padding threw the following:");
    foreach (Exception inner in ex.InnerExceptions)
    {
        System.Console.WriteLine(inner.Message);
        foreach(DictionaryEntry de in inner.Data)
        {
            Console.WriteLine(de.Key.ToString() + ": " + de.Value.ToString());
            
        }
    }
}

try
{
    HttpClient client2 = new();
    client2.DefaultRequestHeaders.Add("Authorization", bearer);
    client2.BaseAddress = new Uri("https://localhost:51443");
    Task<string> httpTask2 = client2.GetStringAsync("/User");
    string resultWithPadding = httpTask2.Result;
    System.Console.WriteLine("The app instance with the padding returned the following:");
    System.Console.WriteLine(resultWithPadding);
}
catch (AggregateException ex)
{
    System.Console.WriteLine("The app instance with the padding threw the following:");
    foreach (Exception inner in ex.InnerExceptions)
    {
        System.Console.WriteLine(inner.Message);
        foreach (DictionaryEntry de in inner.Data)
        {
            Console.WriteLine(de.Key.ToString() + ": " + de.Value.ToString());

        }
    }
}

while (true) Thread.Sleep(1000);