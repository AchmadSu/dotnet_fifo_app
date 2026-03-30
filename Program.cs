using System.Security.Cryptography.Xml;
using FifoApi.Data;
using FifoApi.DTOs;
using FifoApi.Extensions.Controllers;
using FifoApi.Infrastructure.Cache.Products;
using FifoApi.Infrastructure.Cache.Sales;
using FifoApi.Infrastructure.Messaging;
using FifoApi.Infrastructure.Messaging.KafkaConsumer.Products;
using FifoApi.Infrastructure.Messaging.KafkaConsumer.Sales;
using FifoApi.Interface.CacheInterface;
using FifoApi.Interface.KafkaInterface;
using FifoApi.Interface.ProductInterface;
using FifoApi.Interface.SaleInterface;
using FifoApi.Interface.StockInterface;
using FifoApi.Interface.UserInterface;
using FifoApi.Models;
using FifoApi.Repositories.ProductRepository;
using FifoApi.Repositories.SaleRepository;
using FifoApi.Repositories.StockRepository;
using FifoApi.Repositories.UserRepository;
using FifoApi.Service.CacheService;
using FifoApi.Service.ProductService;
using FifoApi.Service.SaleService;
using FifoApi.Service.StockService;
using FifoApi.Service.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FIFO API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token!",
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
            new string[]{}
        }
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var operationResult = OperationResult<object>.BadRequest(
                "Validation failed",
                errors
            );

            return new ObjectResult(operationResult)
            {
                StatusCode = (int)operationResult.StatusCode
            };
        };
    });


builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration["Redis:Connection"];
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IProductCacheHelper, ProductCacheHelper>();
builder.Services.AddScoped<ISaleCacheHelper, SaleCacheHelper>();
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
builder.Services.AddScoped<KafkaMessageDispatcher>();
builder.Services.AddScoped<IKafkaMessageHandler, ProductCreatedHandler>();
builder.Services.AddScoped<IKafkaMessageHandler, ProductUpdatedHandler>();
builder.Services.AddScoped<IKafkaMessageHandler, SaleCreatedHandler>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<KafkaCustomerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    // .WithOrigins("https://frontend-domain.com") // in production
    .SetIsOriginAllowed(origin => true)
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();