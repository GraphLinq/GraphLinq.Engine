using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodeBlock.Engine.API.Entities;
using NodeBlock.Engine.Interop.Plugin;
using NodeBlock.Engine.Interop.Entities;

namespace NodeBlock.Engine.API.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletInformations(int identifierId);
        Task<ManagedWalletBridgeObject> GenerateNewManagedWallet(int walletId, string name);
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

        public async Task<ManagedWalletBridgeObject> GenerateNewManagedWallet(int walletId, string name)
        {
            return await Task.Run(() => EthereumPluginBridge.CreateOrGetWallet(walletId, name));
        }
    }
}
