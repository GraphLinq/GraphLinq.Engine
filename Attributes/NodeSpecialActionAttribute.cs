using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Attributes
{
    public class NodeSpecialActionAttribute : Attribute
    {
        public NodeSpecialActionAttribute(string text, string type, string parameter)
        {
            this.Text = text;
            this.Type = type;
            this.Parameter = parameter;
        }

        public string Text { get; }
        public string Type { get; }
        public string Parameter { get; }
    }
}
