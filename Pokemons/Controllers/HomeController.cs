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

namespace Pokemons.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{		
		private readonly PokemonsContext _context;

		public HomeController(PokemonsContext context)
		{
			_context = context;
		}

		public IActionResult ListPokemons()
		{	
			var pokemons = _context.Pokemon.ToList();
			return View(pokemons);
		}

		[Authorize]
		public IActionResult OrderHistory()
		{
			string email = User.Identity.Name;
			Customer customer = _context.Customer.FirstOrDefault(e => e.Email == email);
			var data = _context.Order.Where(c => c.CustomerId == customer.Id).Include(p => p.Pokemon); 
			var data1 = _context.Order.Include(c => c.Customer).Include(p => p.Pokemon).GroupBy(p => p.Customer);
			return View(data);
		}

		public IActionResult History()
		{
			//var TimeOrder = _context.Order.FirstOrDefault(r => r.Customer.Email == "B@b.ru").TimeOrder;
			//var DataOrder = _context.Order.Include(p => p.Customer).LastOrDefault(r => r.Customer.Email == "B@B").DateOrder;



			var historyOrders = _context.Order.Include(c => c.Customer).GroupBy(p => p.Customer.Email)
				.Select(g => new ViewCustomerModel
				{
					Name = _context.Customer.FirstOrDefault(r => r.Email == g.Key).Name,
					TimeOrder = _context.Order.FirstOrDefault(r => r.Customer.Email == g.Key).TimeOrder,
					DataOrder = _context.Order.FirstOrDefault(r => r.Customer.Email == g.Key).DateOrder,					
					NumberOrder = g.Count() 
				}).ToList();

			return View(historyOrders);
		}


		public IActionResult Order(int Id)
		{
			Pokemon pokemon = _context.Pokemon.FirstOrDefault(u => u.Id == Id);
			if (pokemon!= null)
			{
				return View(pokemon);
			}
			return RedirectToAction(nameof(ListPokemons));
		}

		
		public IActionResult RegisterOrder(int Id)
		{
			Order order = new Order();			
			string email = User.Identity.Name;	
			order.PokemonId = Id;			
			Customer customer = _context.Customer.FirstOrDefault(e => e.Email == email);			
			order.CustomerId = customer.Id;					
			_context.Order.Add(order);
			_context.SaveChanges();
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
