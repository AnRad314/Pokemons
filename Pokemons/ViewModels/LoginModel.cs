using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemons.ViewModels
{
	public class LoginModel
	{
        [EmailAddress(ErrorMessage = "Введенный Email не является адресом электронной почты")]
        [Required(ErrorMessage = "Не указан Email")]        
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")] 
        public string Password { get; set; }
    }
}
