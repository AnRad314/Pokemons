using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pokemons.Data;
using Pokemons.Models;
using Pokemons.ViewModels;

namespace Pokemons.Controllers
{
	public class AccountController : Controller
	{
		private PokemonsContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        public AccountController(PokemonsContext context, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
		{
			_context = context;
            _userManager = userManager;
            _signInManager = signInManager;
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
                var signedUser = await _userManager.FindByEmailAsync(model.Email);
                if (signedUser != null)
				{
                    var result = await _signInManager.PasswordSignInAsync(signedUser.UserName, model.Password, true, false);
                    if (result.Succeeded)
                    {  
                        if (Url.IsLocalUrl(url))
                        {
                            return Redirect(url);
                        }
                        else
                        {
                            return RedirectToAction("ListPokemons", "Home");
                        }
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
                Customer customer = new Customer
                { 
                    Email = model.Email, 
                    UserName = model.Name,
                    PhoneNumber = model.Phone,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(customer, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(customer, false);                   
                   
                    if (Url.IsLocalUrl(url))
                    {
                        return Redirect(url);
                    }
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }  
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()   
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("ListPokemons","Home");
        }

        public IActionResult ExternalAuthentication(string url, string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { url = url });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string url)
        {
            url = url ?? Url.Content("~/");
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded && Url.IsLocalUrl(url))
            {
                return Redirect(url);
            }
            else
            {
                Customer user = new Customer
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    EmailConfirmed = true, 
                    PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone)
                };

                await _userManager.CreateAsync(user);				
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, false);
            }           
            return Redirect(url);
        }

    }
    
}
