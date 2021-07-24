using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokemons.Models;
using Pokemons.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Pokemons.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{		
		private readonly PokemonsContext _context;
		private readonly UserManager<Customer> _userManager;
		public HomeController(PokemonsContext context, UserManager<Customer> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult ListPokemons()
		{	
			var pokemons = _context.Pokemon.ToList();
			return View(pokemons);
		}

		[Authorize]
		public async Task<IActionResult> OrderHistory()
		{
			string name = User.Identity.Name;
			var customer = await _userManager.FindByNameAsync(name);
			var data = _context.Order.Where(c => c.CustomerId == customer.Id).Include(p => p.Pokemon);			
			return View(data);
		}

		public IActionResult History()
		{
			var historyOrders = _context.Order.Include(c => c.Customer).GroupBy(p => p.Customer.Email)
				.Select(g => new ViewCustomerModel
				{
					Name = _context.Order.FirstOrDefault(r => r.Customer.Email == g.Key).Customer.UserName,
					TimeOrder = _context.Order.FirstOrDefault(r => r.Customer.Email == g.Key).TimeOrder,
					DataOrder = _context.Order.FirstOrDefault(r => r.Customer.Email == g.Key).DateOrder,
					NumberOrder = g.Count()
				}).ToList();

			return View(historyOrders);
		}


		public IActionResult Order(int Id)
		{
			Pokemon pokemon = _context.Pokemon.FirstOrDefault(u => u.Id == Id);
			if (pokemon != null)
			{
				return View(pokemon);
			}
			return RedirectToAction(nameof(ListPokemons));
		}


		public async Task<IActionResult> RegisterOrder(int Id)
		{
			Order order = new Order();
			string nameUser = User.Identity.Name;		
			order.PokemonId = Id;
			var customer = await _userManager.FindByNameAsync(nameUser);
			order.CustomerId = customer.Id;
			_context.Order.Add(order);
			await _context.SaveChangesAsync();

			EmailService emailService = new EmailService();
			emailService.SendEmail(customer.Email.ToString(), "Заказ покемона", "Вы оформили заказа на покемона " + _context.Pokemon.FirstOrDefault(e => e.Id == Id).Name);
			
			return RedirectToAction(nameof(History));
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
