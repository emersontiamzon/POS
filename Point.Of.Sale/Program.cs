// See https://aka.ms/new-console-template for more information

using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Point.Of.Sale.Auth.IdentityContext;
using Point.Of.Sale.Persistence.Initializable;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Registrations;
using Point.Of.Sale.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);


//load appsettings configuration
builder.Services.AddOptions();
builder.Services.Configure<PosConfiguration>(builder.Configuration.GetSection("PosConfiguration"));
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<PosConfiguration>>();

//setup keyvault
// var credentials = new ClientSecretCredential(
//     options.Value.KeyVault.DirectoryId,
//     options.Value.KeyVault.ClientId,
//     options.Value.KeyVault.ClientSecret);
//
// builder.Configuration.AddAzureKeyVault(options.Value.KeyVault.KeyVaultUrl,
//     options.Value.KeyVault.ClientId,
//     options.Value.KeyVault.ClientSecret,
//     new DefaultKeyVaultSecretManager());
//
// var client = new SecretClient(new Uri(options.Value.KeyVault.KeyVaultUrl), credentials);
//
// https://www.youtube.com/watch?v=ZXfuxisC0IA


//setup dynamic ef providers
builder.Services.AddDbProvidersRegistration(options.Value);

//register repositories
builder.Services.AddRepositoriesRegistration();

builder.Services.AddIdentity<ServiceUser, IdentityRole>()
    .AddEntityFrameworkStores<UsersDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));


//setup compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Optimal; });

//scan assemblies with scrutor
// builder.Services.AddScrutorRegistration();

//register mediatr
builder.Services.AddMediatrRegistration();


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = $"{options.Value.General.ServiceName}-{options.Value.General.ServiceName}",
        ValidAudience = options.Value.General.ServiceName,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.General.SecretKey)),
    };
});

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo {Title = "Kodelev8 POS Service API", Version = "v1"});
    s.AddSecurityDefinition(
        "JWT", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
        });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT",
                },
            },
            new string[] { }
        },
    });
});

builder.Services.AddAuthorization(
    options => options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).Build()
);



//register controllers
builder.Services.AddControllersRegistration();

// Setup OpenTelemetry Tracing w/ honeybcomb
builder.AddOpenTelemetryAndLoggingRegistration(options.Value);

// setup fluent email
builder.Services.AddEmailRegistrration(options.Value.Smtp);

// builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
IdentityModelEventSource.ShowPII = true;

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
// app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dojo App"));
app.UseSwaggerUI();
//}

// app.UseHttpsRedirection();
//
// app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Map("/exception", () => { throw new InvalidOperationException("Sample Exception"); });

//initialize and apply database migrations
var initializables = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(y => typeof(IInitializable).IsAssignableFrom(y) && !y.IsInterface);

// if (initializables is not null || initializables.Any())
// {
//     foreach (var init in initializables)
//     {
//         try
//         {
//             var instance = (IInitializable) Activator.CreateInstance(init)!;
//             Task.Run(() => instance.Initialize()).Wait();
//         }
//         catch
//         {
//             //ignore errors
//         }
//     }
// }

app.Run();
