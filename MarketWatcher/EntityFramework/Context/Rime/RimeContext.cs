using Rime.ADO.Classes;
using System.Data.Entity;

namespace MarketWatcher.EntityFramework.Context.Rime
{
	public class RimeContext : DbContext
	{
		public RimeContext() : base("Rime") { }
		public virtual DbSet<Token> Tokens { get; set; }
		public virtual DbSet<Pendulum> Pendulums { get; set; }
		public virtual DbSet<PendulumRarity> PendulumRarities { get; set; }
		public virtual DbSet<Puurrties> Puurrties { get; set; }
		public virtual DbSet<PuurrtiesRarity> PuurrtiesRarities { get; set; }
	}
}
