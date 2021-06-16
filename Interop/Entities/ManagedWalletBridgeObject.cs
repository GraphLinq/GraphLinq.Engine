using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Interop.Entities
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
}
