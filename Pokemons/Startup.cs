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
					options.AppId = "2916855048576720";
					options.AppSecret = "231d69df5dadbed37dc067e7155ddfa5";
				})
				.AddGoogle(options =>
				{
					options.ClientId = "206798487298-6k8k15gjfvq8ahlf1tcbq55lncido5uq.apps.googleusercontent.com";
					options.ClientSecret = "qJ_XZU2De4YqUxhKP6z_k8c3";
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
