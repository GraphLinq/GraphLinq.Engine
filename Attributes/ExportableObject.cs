using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class ExportableObject : Attribute
    {
        public string Name { get; }

        public ExportableObject(string name)
        {
            Name = name;
        }
    }
}
