using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Pokemons.Models;

namespace Pokemons.Models
{
	public class Order
	{
		public int Id { get; set; }

		[DataType(DataType.Time)]
		[Display(Name = "Время заказа")]		
		public DateTime TimeOrder { get; set; } = DateTime.Now;

		[DataType(DataType.Date)]
		[Display(Name = "Дата заказа")]		
		public DateTime DateOrder { get; set; } = DateTime.Today;

		[ForeignKey("Id")]
		public string CustomerId { get; set; }
		public Customer Customer { get; set; }
		public int PokemonId { get; set; }

		[Display(Name = "Покемон")]
		public Pokemon Pokemon { get; set; }
	}
}
