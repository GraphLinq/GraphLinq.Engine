using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Interop.Plugin
{
    public class EthereumPluginBridge
    {
        public class ManagedWalletBridgeObject
        {
            public int Id { get; set; }
            public int WalletId { get; set; }
            public string Name { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
            public string PrivateKeyUnencrypted { get; set; }
            public string Password { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public static ManagedWalletBridgeObject CreateOrGetWallet(int walletId, string name, string password)
        {
            var method = PluginManager.GetExportedMethod("ManagedEthereumWallet.GetOrCreateManagedWallet");
            var managedWallet = method.Invoke(null, new object[] { walletId, name, password });
            var test = managedWallet.GetType();
            var entity = managedWallet.GetType().GetProperty("ManagedWalletEntity").GetGetMethod().Invoke(managedWallet, new object[] { });
            var entityType = entity.GetType();
            return new ManagedWalletBridgeObject()
            {
                Id = (int)entityType.GetProperty("Id").GetGetMethod().Invoke(entity, new object[] { }),
                WalletId = (int)entityType.GetProperty("WalletId").GetGetMethod().Invoke(entity, new object[] { }),
                Name = (string)entityType.GetProperty("Name").GetGetMethod().Invoke(entity, new object[] { }),
                PublicKey = (string)entityType.GetProperty("PublicKey").GetGetMethod().Invoke(entity, new object[] { }),
                PrivateKey = (string)entityType.GetProperty("PrivateKey").GetGetMethod().Invoke(entity, new object[] { }),
                PrivateKeyUnencrypted = (string)entityType.GetMethod("GetPrivateKey").Invoke(entity, new object[] { }),
                Password = (string)entityType.GetProperty("Password").GetGetMethod().Invoke(entity, new object[] { }),
                CreatedAt = (DateTime?)entityType.GetProperty("CreatedAt").GetGetMethod().Invoke(entity, new object[] { }),
                UpdatedAt = (DateTime?)entityType.GetProperty("UpdatedAt").GetGetMethod().Invoke(entity, new object[] { }),
            };
        }
    }
}
