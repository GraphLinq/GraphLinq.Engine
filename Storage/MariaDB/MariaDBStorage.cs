using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore;
using NodeBlock.Engine.Storage.MariaDB.Entities;
using NodeBlock.Engine.Storage.MariaDB.Interfaces;

namespace NodeBlock.Engine.Storage.MariaDB
{
    public class MariaDBStorage : DbContext, IDbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Graph> Graphs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseMySQL(Environment.GetEnvironmentVariable("mariadb_uri"));
            }
        }

        public MariaDBStorage(DbContextOptions<MariaDBStorage> options) : base(options) { }

        public async Task<Wallet> FetchWalletById(int id)
        {
            return await this.Wallets.FirstOrDefaultAsync(x => x.id_wallet == id);
        }

        public async Task<Graph> FetchGraphByHash(string hash)
        {
            return await this.Graphs.FirstOrDefaultAsync(x => x.hash_graph == hash);
        }

        public async void RemoveGraphByHash(string hash)
        {
            var graph = await FetchGraphByHash(hash);
            if (graph != null)
                this.Graphs.Remove(graph);
            await this.SaveChangesAsync();
        }
    }
}
