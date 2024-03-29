using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StockAPI.Authorization;
using StockAPI.Controllers;
using StockAPI.Entities;
using StockAPI.Middleware;
using StockAPI.Models;
using StockAPI.Models.Validators;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;                
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
             /*dzieki Configuration, kt�ra jest cz�ci� klasy Startup, jeste�my w stanie odnie�� si� do pliku appsetting.json.
             Na obiekcie konfiguracji mo�emy odnie�� si� do konkretnej sekcji wywo�uj�� metod� GetSection,
             do kt�rej przekazujemy nazw� danej sekcji. Odnosz�c si� do tej sekcji mo�emy po��czy� warto�ci z tej sekcji
             do zmiennej authenticationSettings. Teraz warto�ci w pliku appsettings.json s� dost�pne na obiekcie authenticationSettings*/
            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("HasNationality", builder =>
                    builder.RequireClaim("Nationality", "German", "Polish"));
                options.AddPolicy("Atleast20", builder =>
                    builder.AddRequirements(new MiniumAgeRequirement(20)));
                options.AddPolicy("Atleast2Stocks", builder =>
                    builder.AddRequirements(new MinimumStocksCreatedRequirement(2)));
                
            });
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddTransient<IMarketService, MarketService>();
            services.AddTransient<IObservedService, ObservedService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IStockScraper, StockScraper>();
            services.AddControllers().AddFluentValidation();          
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StockDbConnection")));
            services.AddScoped<MarketSeeder>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<MarketQuery>, MarketQueryValidator>();
            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, MinimumStocksCreatedRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder =>

                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(Configuration["AllowedOrigins"])
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MarketSeeder seeder)
        {
            app.UseResponseCaching();
            app.UseStaticFiles(); //serving static files from wwwroot
            app.UseCors("FrontEndClient");
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();
            app.UseAuthentication(); //ka�dy request wys�any przez klienta b�dzie podlega� autentykacji
            app.UseHttpsRedirection(); //je�li klient API wy�le zapytanie bez protoko�u https
            //to jego zapytanie, zostanie automatycznie przekierowane na adres z protoko�em https

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
