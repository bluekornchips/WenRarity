using Rime.ADO.Classes;
using Rime.ADO.Classes.Listings;
using Rime.ADO.Classes.Tokens;
using Rime.ADO.Classes.Tokens.Rarity;
using Rime.Store.Stores;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rime.ADO
{
    public class RimeContext : DbContext
    {
        public RimeContext(): base("Rime") { }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Lioness> Lioness { get; set; }
        public virtual DbSet<LionessRarity> LionessRarity { get; set; }
        public virtual DbSet<ChilledKong> ChilledKong { get; set; }
        public virtual DbSet<ChilledKongRarity> ChilledKongRarity { get; set; }
        public virtual DbSet<JPGStoreListing> JPGStoreListings { get; set; }
        public virtual DbSet<CheekyUnt> CheekyUnts { get; set; }
        public virtual DbSet<HappyHopper> HappyHoppers { get; set; }
        public virtual DbSet<HappyHoppersRarity> HappyHopperRaritys { get; set; }
        public virtual DbSet<WinterNaru> WinterNarus { get; set; }
        public virtual DbSet<WinterNaruRarity> WinterNaruRaritys { get; set; }
        public virtual DbSet<BrightPal> BrightPals { get; set; }
        public virtual DbSet<BrightPalRarity> BrightPalRarities { get; set; }
        public virtual DbSet<Rave> Raves { get; set; }
        public virtual DbSet<RaveRarity> RaveRaritys { get; set; }
        public virtual DbSet<GhostWatch> GhostWatches { get; set; }
        public virtual DbSet<GhostWatchesRarity> GhostWatchesRarity { get; set; }
        public virtual DbSet<Puurrties> Puurrties { get; set; }
        public virtual DbSet<PuurrtiesRarity> PuurrtiesRarities { get; set; }
        public virtual DbSet<Pendulum> Pendulums { get; set; }
        public virtual DbSet<PendulumRarity> PendulumRarities { get; set; }
        public virtual DbSet<ElMatador> ElMatadors { get; set; }
        public virtual DbSet<ElMatadorRarity> ElMatadorRarities { get; set; }
    }
}