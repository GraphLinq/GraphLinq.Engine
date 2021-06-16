using NodeBlock.Engine.Interop.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Interop.Plugin
{
    public class EthereumPluginBridge
    {
        public static ManagedWalletBridgeObject CreateOrGetWallet(int walletId, string name)
        {
            var method = PluginManager.GetExportedMethod("ManagedEthereumWallet.GetOrCreateManagedWallet");
            var managedWallet = method.Invoke(null, new object[] { walletId, name });
            var entity = managedWallet.GetType().GetProperty("ManagedWalletEntity").GetGetMethod().Invoke(managedWallet, new object[] { });
            var entityType = entity.GetType();
            return new ManagedWalletBridgeObject()
            {
                Id = (int)entityType.GetProperty("Id").GetGetMethod().Invoke(entity, new object[] { }),
                WalletId = (int)entityType.GetProperty("WalletId").GetGetMethod().Invoke(entity, new object[] { }),
                Name = (string)entityType.GetProperty("Name").GetGetMethod().Invoke(entity, new object[] { }),
                PublicKey = (string)entityType.GetProperty("PublicKey").GetGetMethod().Invoke(entity, new object[] { }),
                PrivateKey = (string)entityType.GetProperty("PrivateKey").GetGetMethod().Invoke(entity, new object[] { }),
                PrivateKeyUnencrypted = (string)managedWallet.GetType().GetMethod("GetPrivateKey").Invoke(managedWallet, new object[] { }),
                Password = (string)entityType.GetProperty("Password").GetGetMethod().Invoke(entity, new object[] { }),
                CreatedAt = (DateTime?)entityType.GetProperty("CreatedAt").GetGetMethod().Invoke(entity, new object[] { }),
                UpdatedAt = (DateTime?)entityType.GetProperty("UpdatedAt").GetGetMethod().Invoke(entity, new object[] { }),
            };
        }
    }
}
