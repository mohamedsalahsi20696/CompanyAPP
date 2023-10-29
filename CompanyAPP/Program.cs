using BusinessLayer.Interfaces;
using BusinessLayer.Repositories;
using CompanyAPP.MapperProfiles;
using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CompanyAPP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            #region Configure Services that allow Dependanct Injection

            builder.Services.AddControllersWithViews();

            // dependancy injection of ConnectionString and CompanyDbContext
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // dependancy injection of DepartmentController
            // this is meaning when any one define reference of IDepartmentRepository send to opject from DepartmentRepository
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // comment this line because i used UntiOfWork
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // comment this line because i used UntiOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(d => d.AddProfile(new DepartmentProfile()));
            builder.Services.AddAutoMapper(d => d.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(d => d.AddProfile(new RoleProfile()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(optins =>
            {
                optins.Password.RequireNonAlphanumeric = true;// should password have #,@,,% any special charcter
                optins.Password.RequireDigit = true;// should password have number
                optins.Password.RequireUppercase = true; // should password have uppercase
                optins.Password.RequiredLength = 10; // should password have 10 char

            }).AddEntityFrameworkStores<CompanyDbContext>()
              .AddDefaultTokenProviders();  // default tokens

            //builder.Services.AddScoped<UserManager<ApplicationUser>>();
            //builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            //builder.Services.AddScoped<RoleManager<IdentityRole>>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "Account/Login";
                        options.AccessDeniedPath = "Home/Error";
                    });   // this line to meaning DI of ( UserManager , SignInManager , RoleManager ) and create token save it in cookies


            #endregion

            var app = builder.Build();
            var env = builder.Environment;
            #region Configure Http Request Pipelines

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // to redirect http request to https
            app.UseHttpsRedirection();
            // to use all resources like css or bootstrap
            app.UseStaticFiles();
            // routing table
            app.UseRouting();

            // 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            #endregion

            app.Run();
        }



    }
}
