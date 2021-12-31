using System.Text;
using KevinBlogApi.Core.Model.Interface;
using KevinBlogApi.Data;
using KevinBlogApi.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using KevinBlogApi.Web.Hubs;
using AutoMapper;
using KevinBlogApi.Web.Profiles;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            services.AddCors(options => {
                options.AddDefaultPolicy(
                builder => {
                    builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
                });
            });

            var key = Configuration.GetSection("AppSettings")["Key"];
            services.AddSpaStaticFiles(Options => {
                Options.RootPath = "BlogVue/dist";
            });
            services.AddDbContext<KevinBlogDataContext>(Options => {
                Options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
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
            services.AddAutoMapper(typeof(ModelProfile));
            services.AddSignalR();
            services.AddControllers();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseCors();            
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllers();
            });
            app.UseSpa(Options => {
                Options.Options.SourcePath = "BlogVue/dist";
            });
        }
    }
}
