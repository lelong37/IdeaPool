#region

using System.Reflection;
using System.Text;
using IdeaPool.Application.Infrastructure;
using IdeaPool.Application.Users.Commands;
using IdeaPool.Domain.Infrastructure;
using IdeaPool.Domain.Models;
using IdeaPool.Services;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace IdeaPool.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }
        public AppSettings AppSettings { get; set; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if(env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Get}/{id?}",
                    defaults: );
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            AppSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var ideaPoolContext = context.HttpContext.RequestServices.GetRequiredService<IdeaPoolContext>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = await ideaPoolContext.User.FindAsync(userId);
                        if(user == null) context.Fail("Unauthorized");
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<IdeaPoolContext>(options =>
            {
                string connectionString;

                if(!AppSettings.UseInMemoryDatabase.GetValueOrDefault(false))
                {
                    connectionString = Configuration.GetConnectionString("IdeaPoolContextSql");
                    options.UseSqlServer(connectionString);
                }
                else
                {
                    connectionString = Configuration.GetConnectionString("IdeaPoolContextInMemory");

                    var connection = new SqliteConnection(connectionString);
                    connection.Open();

                    options.UseSqlite(connection);
                }
            });

            services.AddScoped<IUserService, UserService>();
            services.AddMediatR(typeof(CreateUserCommand).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
        }
    }
}