using Eob_Web.Core.Services;
using Eob_Web.Frontend.Areas.Identity;
using Eob_Web.Frontend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Eob_Web.Core.Mappers;
using Hangfire;
using System.Diagnostics;
using Eob_Web.Core;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.StaticFiles;

namespace Eob_Web.Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Identity
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Auth")));

            services.AddDefaultIdentity<ApplicationUser>(options =>
                    options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
                options.AddPolicy("verified", policy => policy.RequireClaim("verified")));

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(3));

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

            //JWT Authentication
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SigningKey"]))
                    };
                });

            services.AddHttpContextAccessor();

            // Hangfire
            services.AddHangfire(options =>
                options.UseSqlServerStorage(Configuration.GetConnectionString("Hangfire")));

            services.AddHangfireServer();

            // Services
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IEob_Service, Eob_Service>();
            services.AddTransient<ICompany_Service, Company_Service>();
            services.AddTransient<IGroup_Service, Group_Service>();
            services.AddTransient<IUser_Service, User_Service>();
            services.AddTransient<ISubscription_Service, Subscription_Service>();
            services.AddTransient<IEmailSender, Email_Service>();
            services.AddTransient<IBulkEmailSender, Email_Service>();
            services.AddTransient<IJob_Service, Job_Service>();

            services.AddHttpClient();
            services.AddHttpClient<IZeroTier_Service, ZeroTier_Service>();

            // Mappers
            services.AddScoped<Eob_Mapper>();
            services.AddScoped<Group_Mapper>();
            services.AddScoped<Company_Mapper>();
            services.AddScoped<User_Mapper>();
            services.AddScoped<ZeroTier_Mapper>();

            // Blazor
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // exclude SoftwareFiles
            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/SoftwareFiles"),
                appBuilder => appBuilder.UseStaticFiles());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // File server
            // Unfotunatley, I wasn't able to get JWT token authentication working with StaticFiles.
            // This solution is hacky but it works.
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "SoftwareFiles")),
                RequestPath = "/files",

                OnPrepareResponse = ctx =>
                {
                    string token = ctx.Context.Request.Headers["authorization"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        token = token.Replace("Bearer ", "");

                        var token_Validation_Parameters = new TokenValidationParameters
                        {
                            RequireExpirationTime = true,
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SigningKey"]))
                        };

                        var token_Handler = new JwtSecurityTokenHandler();
                        var principal = token_Handler.ValidateToken(token, token_Validation_Parameters, out var validatedToken);

                        // if token authentication is invalid
                        if (!principal.Identity.IsAuthenticated)
                        {
                            Forbid(ctx);
                        }

                        return;
                    }

                    // if user authentication is invalid
                    if (!ctx.Context.User.Identity.IsAuthenticated || !ctx.Context.User.HasClaim(x => x.Type == "verified"))
                    {
                        Forbid(ctx);
                    }
                }
            });

            // Hangfire
            app.UseHangfireDashboard();

            var job_Service = provider.GetService<IJob_Service>();

            //RecurringJob.AddOrUpdate(
            //    "Subscription_Expiration_Reminder",
            //    () => job_Service.Email_Subscription_Expiration_Remider(),
            //    Cron.Daily);

            //RecurringJob.AddOrUpdate(
            //    "Subscription_Expired",
            //    () => job_Service.Email_Subscription_Expired(),
            //    Cron.Daily);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private void Forbid(StaticFileResponseContext ctx)
        { 
            ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ctx.Context.Response.ContentLength = 0;
            ctx.Context.Response.Body = Stream.Null;
            ctx.Context.Response.Headers.Add("Cache-Control", "no-store");
        }
    }
}
