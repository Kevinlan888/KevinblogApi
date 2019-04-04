using System.Text;
using KevinBlogApi.Core.Model.Interface;
using KevinBlogApi.Data;
using KevinBlogApi.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace KevinBlogApi.Web
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
            var key = Configuration.GetSection("AppSettings")["Key"];
            services.AddSpaStaticFiles(Options => {
                Options.RootPath = "ClientApp/dist";
            });
            services.AddDbContext<KevinBlogDataContext>(Options => {
                Options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddIdentity<IdentityUser, IdentityRole>(Options =>
            {
                Options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<KevinBlogDataContext>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options => {
                Options.RequireHttpsMetadata = true;
                Options.SaveToken = true;
                Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors(Options => {
                Options.AllowAnyHeader();
                Options.AllowAnyMethod();
                Options.AllowAnyOrigin();
                Options.AllowCredentials();
            });            
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSpa(Options => {
                Options.Options.SourcePath = "ClientApp/dist";
            });
        }
    }
}
