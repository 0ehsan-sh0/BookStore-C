using BookStoreApi.BusinessLogicLayer;
using BookStoreApi.BusinessLogicLayer.Interfaces;
using BookStoreApi.BusinessLogicLayer.Interfaces.Admin;
using BookStoreApi.BusinessLogicLayer.Interfaces.Public;
using BookStoreApi.BusinessLogicLayer.Interfaces.UserPanel;
using BookStoreApi.BusinessLogicLayer.LogicLayers.Admin;
using BookStoreApi.BusinessLogicLayer.LogicLayers.Public;
using BookStoreApi.BusinessLogicLayer.LogicLayers.UserPanel;
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
// Database & BLL services DI
// -------------------------
builder.Services.AddSingleton(typeof(BookStoreApi.Database.DapperUtility));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBLLCategory, BLLCategory>();
builder.Services.AddScoped<IBLLCategoryPublic, BLLCategoryPublic>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBLLAuthor, BLLAuthor>();

builder.Services.AddScoped<ITranslatorRepository, TranslatorRepository>();
builder.Services.AddScoped<IBLLTranslator, BLLTranslator>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBLLBook, BLLBook>();
builder.Services.AddScoped<IBLLBookPublic, BLLBookPublic>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IBLLImage, BLLImage>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IBLLComment, BLLComment>();
builder.Services.AddScoped<IBLLUserComment, BLLUserComment>();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IBLLTag, BLLTag>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBLLAuth, BLLAuth>();
builder.Services.AddScoped<IBLLUserPanel, BLLUserPanel>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IBLLUserPayment, BLLUserPayment>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IBLLUserInvoice, BLLUserInvoice>();

builder.Services.AddScoped<IInvoiceBooksRepository, InvoiceBooksRepository>();

builder.Services.AddScoped<IWishListRepository, WishListRepository>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IBLLUserAddress, BLLUserAddress>();

// -------------------------
// JWT service
// -------------------------
builder.Services.AddScoped<JWTService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;

    // Read token from the "access_token" cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = builder.Configuration["JWTConfiguration:Issuer"],
        ValidAudience = builder.Configuration["JWTConfiguration:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:Key"]!)
        )
    };
});

// -------------------------
// Authorization
// -------------------------
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
              .AllowAnyMethod()
              .AllowCredentials();
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
