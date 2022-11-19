using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiplomaService.Database;
using DiplomaService.Repositories.Implementations;
using DiplomaService.Repositories.Interfaces;
using DiplomaService.swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DiplomaService;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    private RsaSecurityKey GetSecurityKey()
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem("-----BEGIN PUBLIC KEY-----MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCxNFEQFDqxkd81dc+R5r90Tit3v9RYuEy5ls5vvHgF9h932MZqYN8GXbVn0SEimxk3NnnlxRbzciqCqDMZIWp08Iscm/isRJpWrD064k3NhK86gsmDIWjxemoUOnrbdduhRfK6WhzmvhQEiO6tlDKAzw6e7rdNqGwBffg2H1h0EQIDAQAB-----END PUBLIC KEY-----");
        return new RsaSecurityKey(rsa);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Asymmetric", options =>
            {
                SecurityKey rsa = GetSecurityKey();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = rsa,
                    ValidIssuer = "mospolytech",
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                };
            });
        
        services.AddDbContext<StudentsContext>(options => 
            options.UseInMemoryDatabase("TestDatabase"));

        services.AddTransient<IStudentAsyncRepository, StudentAsyncRepository>();
        services.AddTransient<IProfessorAsyncRepository, ProfessorAsyncRepository>();
        services.AddTransient<IProjectAsyncRepository, ProjectAsyncRepository>();
        
        services.AddControllers()
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opts.JsonSerializerOptions.IncludeFields = true;
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                // opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SchemaFilter<SwaggerIgnoreFilter>();
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        
        // if (env.IsDevelopment())
        // {
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
                options.RouteTemplate = "swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diploma service API v1");
                c.RoutePrefix = "swagger";
            });

        // }

        app.UseHttpsRedirection();
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}