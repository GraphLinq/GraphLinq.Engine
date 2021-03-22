using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodeBlock.Engine.API.Entities;

namespace NodeBlock.Engine.API.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletInformations(int identifierId);
    }

    public class WalletService : IWalletService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Wallet> wallets = new List<Wallet> {
            new Wallet { IdentifierId = 1 }
        };

        public async Task<Wallet> GetWalletInformations(int identifierId)
        {
            return await Task.Run(() => wallets.Find(x => x.IdentifierId == identifierId));
        }
    }
}
