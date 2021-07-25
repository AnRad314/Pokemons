using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemons.ViewModels
{
	public class ViewCustomerModel
	{	
		[Display(Name = "Время последнего заказа")]
		[DataType(DataType.Time)]
		public DateTime TimeOrder { get; set; }

		[Display(Name = "Дата последнего заказа")]
		[DataType(DataType.Date)]
		public DateTime DataOrder { get; set; }

		[Display(Name = "Имя заказчика")]
		public string Name { get; set; }
		public int NumberOrder { get; set; }
	}
}
