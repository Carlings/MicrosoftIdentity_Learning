using Microsoft.AspNetCore.Authorization;
using System;
using MicrosoftIdentity_Learning.Pages.Account.AuthorizationCustomRequirements;

namespace MicrosoftIdentity_Learning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.ExpireTimeSpan = new TimeSpan(0, 0, 200);
                options.AccessDeniedPath = "/Account/AccessDenied/AccessDenied";
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HROnly", policy => { policy.RequireClaim("Department", "HR"); });


                options.AddPolicy("HRManagementOnly", policy => { policy
                    .RequireClaim("Department", "HR")
                    .RequireClaim("Role", "HRManager")
                    .Requirements.Add(new HRManagerProbationRequirement(3));
                    });
            });

            builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();
            builder.Services.AddHttpClient("WebApiForecastClient", configure =>
            {
                configure.BaseAddress = new Uri("https://localhost:7013");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}