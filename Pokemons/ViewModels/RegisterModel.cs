using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemons.ViewModels
{
	public class RegisterModel
	{
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength (60, MinimumLength = 2)]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Не указан e-mail")]
        public string Email { get; set; }

        [Phone]
        [Required(ErrorMessage = "Не указан номер телефона")]        
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(15, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string ConfirmPassword { get; set; }
    }
}
