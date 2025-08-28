using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // add db context   
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    )
                );

            // add session
            //builder.Services.AddSession(
            //    option => option.IOTimeout = TimeSpan.FromMinutes(10)
            //    );

            //builder.Services.AddScoped<>(); // add interfaces

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();

            //app.UseSession();
            //app.UseCookiePolicy();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
