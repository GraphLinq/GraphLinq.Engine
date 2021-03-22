using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Interop.Plugin
{
    public class BasePlugin
    {
        public virtual void Load() {
            throw new NotImplementedException();
        }
    }
}
