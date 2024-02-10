
using Api.Models.SQLServer;
using Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Api.ViewModels;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Api.Components;
using Api.Repositories;
using Api.Authentication;
using Service.WebApi.Catalog.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRTokenRepository, RTokenRepository>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IDecryptManager, DecryptManager>();
            services.AddScoped<IRefreshToken, RefreshToken>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<ICookies, Cookies>();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //create sendiri DI untuk configurasi database dbPortalPace
            services.AddDbContext<dbRmTools_Context>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("dbSqlServer")));

            //add cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Services", Version = "v1", Description = "Web Api" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                    Array.Empty<string>()
                }
                });
            });

            // Get credential from appsetting.json file
            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<CredentialAttr>(appSettingSection);

            // Extract appsetting values
            var appSettings = appSettingSection.Get<CredentialAttr>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var issuer = appSettings.Issuer;


            // Set authentication settings
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
             .AddJwtBearer(x =>
             {
                 x.RequireHttpsMetadata = false;
                 x.SaveToken = true;
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     // Set validation to be challanged by our credential
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = true,
                     ValidIssuer = issuer,
                     ValidateAudience = false
                 };
             })
            .AddCookie(options =>
             {
                 options.LoginPath = "/api/Login/Authentication"; // Sesuaikan jalur login Anda
                 options.LogoutPath = "/api/Login/Logout"; // Sesuaikan jalur logout Anda
             });


            services.AddRazorPages();
            services.AddHttpContextAccessor();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/ServiceRMT/RMT_ApiLogin/swagger/v1/swagger.json", "Production");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "local");
                c.DocumentTitle = "Documentation";
                c.DocExpansion(DocExpansion.None);
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

    }
}
