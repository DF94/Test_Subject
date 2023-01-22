using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using OnboardingApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnboardingApp.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnboardingApp.Data.SqlQuerys;

namespace OnboardingAPI
{
    public class Startup
    {
        private readonly IConfiguration Config;
        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<OnboardingContext>(
             opt => opt.UseSqlServer(Config.GetConnectionString("OnboardingConnect"))
            .EnableSensitiveDataLogging()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            //services.AddIdentity<User, IdentityRole>()
            //.AddEntityFrameworkStores<OnboardingContext>()
            //.AddDefaultTokenProviders();


            // Add SQL interface
            services.AddScoped<ISQLFectch, SQLFectch>();

            services.AddIdentity<User, IdentityRole<int>>(x =>
            {
                x.Password.RequiredLength = 5;
                x.Password.RequireUppercase = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonAlphanumeric = false;
                x.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<OnboardingContext>();


            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            
            .AddJwtBearer(options =>
            {
                //options.SaveToken = true;
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //ValidateIssuer = true,
                    //ValidateAudience = true,
                    ValidAudience = Config["Token:Audience"],
                    ValidIssuer = Config["Token:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Token:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager, OnboardingContext obContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            Init.SeedData(roleManager, userManager, obContext);

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
