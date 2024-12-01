using APIStickerAlbum.Configurations;
using APIStickerAlbum.Context;
using APIStickerAlbum.Extensions;
using APIStickerAlbum.Filters;
using APIStickerAlbum.Interfaces;
using APIStickerAlbum.Models;
using APIStickerAlbum.Repositories;
using APIStickerAlbum.Services;
using APIStickerAlbum.Services.Middlewares;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var sqlConn = builder.Configuration["ConnectionStrings:StickerAlbum:SqlDb"];

SerilogSetup.ConfigureSerilog(builder.Configuration);

builder.Host.UseSerilog();

builder.Services.AddHttpClient();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

var CorsPolicy = "_CorsPolicy";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options => options.AddPolicy(name: CorsPolicy, policy =>
{
    policy.WithOrigins(allowedOrigins!)
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sticker Album API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
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

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<APIStickerAlbumDbContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Chave secreta inv�lida");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<APIStickerAlbumDbContext>(options => options.UseSqlServer(sqlConn));

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IAlbumShareRepository, AlbumShareRepository>();
builder.Services.AddScoped<IAlbumShareService, AlbumShareService>();
builder.Services.AddScoped<IStickerRepository, StickerRepository>();
builder.Services.AddScoped<ILearnersAlbumRepository, LearnersAlbumRepository>();
builder.Services.AddScoped<ILearnersStickerRepository, LearnersStickerRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

if (builder.Configuration["Storage:Local"] == "cloud")
{
    builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration["ConnectionStrings:StickerAlbum:AzureStorage"]));

    builder.Services.AddScoped<IStorageService, AzureBlobStoreService>(x =>
        new AzureBlobStoreService(
            x.GetRequiredService<BlobServiceClient>(),
            builder.Configuration["Storage:AzureStorage:ContainerName"]!));
}
else
{
    builder.Services.AddScoped<IStorageService, LocalStorageService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseStaticFiles();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(CorsPolicy);
app.UseAuthorization();
app.UseMiddleware<OwnershipAlbumMiddleware>();
app.MapControllers();
app.Run();