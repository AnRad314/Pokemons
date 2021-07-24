using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokemons.Models;

namespace Pokemons.Data
{
	public class PokemonsContext : DbContext
	{		
		public PokemonsContext(DbContextOptions<PokemonsContext> options) :base (options)
		{
			Database.EnsureCreated();
		}
		public DbSet <Order> Order { get; set; }
		public DbSet <Customer> Customer { get; set; }
		public DbSet <Pokemon> Pokemon { get; set; }
	}
}
