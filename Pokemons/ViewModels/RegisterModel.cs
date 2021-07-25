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
        [StringLength (20, MinimumLength = 2, ErrorMessage = "Имя должно состоять из 2 до 20 символов") ]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Введенный Email не является адресом электронной почты")]
        [Required(ErrorMessage = "Не указан e-mail")]
        public string Email { get; set; }

        [Phone]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Номер телефона должен состоять из 10 цифр")]
        [Required(ErrorMessage = "Не указан номер телефона")]        
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(15, MinimumLength = 8)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")] 
        public string ConfirmPassword { get; set; }
    }
}
