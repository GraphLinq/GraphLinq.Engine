using Microsoft.EntityFrameworkCore;
using NodeBlock.Engine.Storage.MariaDB.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Storage.MariaDB.Interfaces
{
    public interface IDbContext
    {
        DbSet<Wallet> Wallets { get; set; }
    }

}
