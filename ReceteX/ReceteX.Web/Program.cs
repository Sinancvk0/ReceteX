using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ReceteX.Data;
using ReceteX.Repository.Shared.Abstract;
using ReceteX.Repository.Shared.Concrete;


namespace ReceteX.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//Programımızın login işlemi gerektirdiğine  dair autho tanımlıyoruz.

			var builder = WebApplication.CreateBuilder(args);

			

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

			builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("connstr")));


			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}