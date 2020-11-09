
using System.Text;

using AutoMapper;

using ContactAPI.Attributes;
using ContactAPI.Data.Models;
using ContactAPI.Data.Repository;
using ContactAPI.Extentions;
using ContactAPI.Models;
using ContactAPI.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ContactAPI
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
            services.AddDbContext<ContactsDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ContactsDB")));

            services.AddAutoMapper(typeof(Startup));

            services.AddHttpContextAccessor();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();

            services.ConfigureSwaggerUI();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsContactOwner", policy =>
                    policy.Requirements.Add(new EditOwnerPermision(ContactEnum.TrackableTypes.Contacts)));

                options.AddPolicy("IsSkillOwner", policy =>
                    policy.Requirements.Add(new EditOwnerPermision(ContactEnum.TrackableTypes.Skills)));

            });

            services.AddScoped<IAuthorizationHandler, IsOwnerHandler>();

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
