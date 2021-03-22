using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeGasConfiguration : Attribute
    {
        public NodeGasConfiguration(string blockGasPrice)
        {
            BlockGasPrice = BigInteger.Parse(blockGasPrice);
        }

        public BigInteger BlockGasPrice { get; }
    }
}
