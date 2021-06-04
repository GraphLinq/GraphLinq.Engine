using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeIDEParametersAttribute : Attribute
    {
        public bool Hidden = false;
        public bool IsSecretInput = false;
        public bool IsScriptInput = false;
        public string ScriptType = "lua";
    }
}
