using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SCMR_Api.Hubs;

namespace SCMR_Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .Build());
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        LifetimeValidator = (before, expires, token, param) =>
                        {
                            return expires > DateTime.UtcNow;
                        },
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/chatHub") || path.StartsWithSegments("/us"))
                            )
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };

                });



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(op =>
                {
                    op.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    op.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    op.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
                o.HandshakeTimeout = TimeSpan.FromMinutes(10);
                o.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
                o.KeepAliveInterval = TimeSpan.FromMinutes(10);
            });

            services.AddSingleton(typeof(HubLifetimeManager<ChatHub>), typeof(NotificationHubLifetimeManager<ChatHub>));


            var connectionString = Configuration["ConnectionStrings:Base"];

            services.AddDbContext<Data.DbContext>
                    (options => options.UseSqlServer(Configuration["ConnectionStrings:Base"]));

            services.AddHttpContextAccessor();

            services.AddHostedService<TimedLogService>();
            services.AddScoped<IScopedLogProcessingService, ScopedLogProcessingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles")),
                RequestPath = "/UploadFiles"
            });

            app.UseAuthentication();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<Data.DbContext>();
                context.Database.Migrate();
            }

            // app.UseCors("CorsPolicy");

            app.UseCors(builder =>
            {
                // builder.AllowAnyHeader()
                //     .AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(c => true);

                var validSites = Configuration["Sites:ValidSites"].Split(",");

                foreach (var site in validSites)
                {
                    builder.WithOrigins(site)
                        .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                }
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");

                routes.MapHub<UserHub>("/us");
            });

            app.UseMvc();
        }
    }
}
