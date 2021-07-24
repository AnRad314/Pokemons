using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Pokemons.Data;
using Pokemons.Models;
using Pokemons.ViewModels;

namespace Pokemons.Controllers
{
	public class AccountController : Controller
	{
		private PokemonsContext _context;
		public AccountController(PokemonsContext context)
		{
			_context = context;
		}

        [HttpGet]
        public IActionResult Login(string url)
        {
            ViewBag.Url = url;
            return View();
        }     
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string url )
        {
            url = url ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
               Customer customer =  _context.Customer.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (customer != null)
                {
                    await Authenticate(customer); 

                    if (Url.IsLocalUrl(url))
                    {                       
                        return Redirect(url);
                    }
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }
       
        [HttpGet]
        public IActionResult Register(string url)
        {
            ViewBag.Url = url;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string url)
        {
            url = url ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                Customer customer = _context.Customer.FirstOrDefault(u => u.Email == model.Email);
                if (customer == null)
                {
                    customer = new Customer 
                    { 
                        Name = model.Name, 
                        Email = model.Email,
                        Password = model.Password,
                        ContactPhone = model.Phone 
                    };
                  
                    _context.Customer.Add(customer);
                    await _context.SaveChangesAsync();

                    await Authenticate(customer); 

                    if (Url.IsLocalUrl(url))
                    {
                        return Redirect(url);
                    }
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");

            return View(model);
        }

        private async Task Authenticate(Customer customer)
        {           
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, customer.Email),
                new Claim("Login", customer.Name)
            };  
            
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie");           
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()   
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("ListPokemons","Home");
        }

		
	}
}
