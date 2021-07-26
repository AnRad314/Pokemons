using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokemons.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using Pokemons.Models;
using Microsoft.AspNetCore.Identity;

namespace Pokemons
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
			
			services.AddDbContext<PokemonsContext> (options =>
			options.UseSqlServer(Configuration.GetConnectionString("PokemonsContext")));

			services.AddIdentity<Customer, IdentityRole>(opts =>
			{
				opts.Password.RequiredLength = 8;  
				opts.Password.RequireNonAlphanumeric = false;  
				opts.Password.RequireLowercase = false; 
				opts.Password.RequireUppercase = false; 
				opts.Password.RequireDigit = false;
				
			}) 
			  .AddEntityFrameworkStores<PokemonsContext>();

			services.AddAuthentication()
				.AddFacebook(options =>
				{
					options.AppId = Configuration["Authentication:Facebook:AppId"];
					options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
				})
				.AddGoogle(options =>
				{
					options.ClientId = Configuration["Authentication:Google:ClientId"];
					options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
				});
			services.AddControllersWithViews();
		}

		
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");				
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=ListPokemons}/{id?}");
			});
		}
	}
}
