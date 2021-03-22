using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Engine.Enums;

namespace NodeBlock.Engine.Storage.Redis.Entities
{
    class ActiveGraphStorage
    {
        public string Hash { get; set; }
        public GraphStateEnum GraphLastState { get; set; }
    }
}
