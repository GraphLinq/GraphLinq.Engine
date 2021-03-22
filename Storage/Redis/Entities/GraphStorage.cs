using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Engine.Enums;

namespace NodeBlock.Engine.Storage.Redis.Entities
{
    public class GraphStorage
    {
        public string StoredHash { get; set; }
        public string CompressedBytes { get; set; }

        public int WalletIdentifierOwner { get; set; }

        public GraphStateEnum StateGraph { get; set; }
    }
}
