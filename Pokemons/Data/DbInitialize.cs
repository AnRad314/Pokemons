using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemons.Models;
using Pokemons.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Pokemons.Data
{
	public static class DbInitialize
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new PokemonsContext(
				serviceProvider.GetRequiredService<DbContextOptions<PokemonsContext>>()))

			if (!context.Pokemon.Any())
			{
				context.Pokemon.AddRange(
					new Pokemon
					{
						Name = "Pikachu",
						Image = "/img/Pikachu.jpg",
					},
					new Pokemon
					{
						Name = "Jigglypuff",
						Image = "/img/Jigglypuff.jpg",
					},
					new Pokemon
					{
						Name = "Charmander",
						Image = "/img/Charmander.jpg",
					});
				context.SaveChanges();
			}
		}
	}
}
