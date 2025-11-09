using BookStoreApi.BusinessLogicLayer;
using BookStoreApi.BusinessLogicLayer.Admin;
using BookStoreApi.Database.Interfaces;
using BookStoreApi.Database.Repositories;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Add services to container
// -------------------------

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// -------------------------
// Database & BLL services
// -------------------------
builder.Services.AddSingleton(typeof(BookStoreApi.Database.DapperUtility));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<BLLCategory>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<BLLAuthor>();

builder.Services.AddScoped<ITranslatorRepository, TranslatorRepository>();
builder.Services.AddScoped<BLLTranslator>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<BLLBook>();
builder.Services.AddScoped<BookStoreApi.BusinessLogicLayer.Public.BLLBook>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<BLLImage>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<BLLComment>();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<BLLTag>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<BLLAuth>();

// -------------------------
// JWT service
// -------------------------
builder.Services.AddScoped<JWTService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWTConfiguration:Issuer"],
        ValidAudience = builder.Configuration["JWTConfiguration:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RoleClaimType = ClaimTypes.Role
    };
});
builder.Services.AddAuthorization();

// -------------------------
// SMS.IR service (Typed HttpClient)
// -------------------------
builder.Services.AddHttpClient<ISmsIrService, SmsIrService>(client =>
{
    // Optional: default request headers or timeout
    client.Timeout = TimeSpan.FromSeconds(30);
});

// -------------------------
// CORS
// -------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration["Frontend:URL"]!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// -------------------------
// HTTP request pipeline
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
