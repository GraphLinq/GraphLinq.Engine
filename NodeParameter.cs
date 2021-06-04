using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NodeBlock.Engine
{
    public class NodeParameter : ICloneable
    {
        public string Id { get; set; }
        [JsonIgnore]
        public Node Node { get; }
        public string Name { get; set; }
        public object Value { get; set; }
        public Type ValueType { get; set; }
        public bool IsIn { get; set; }
        public bool IsDynamic { get; set; }
        public NodeParameter Assignments { get; set; }
        public bool IsReference { get; set; }

        public NodeParameter(Node node, string name, Type valueType, bool isIn, object value = null, string id = "", bool isDynamic = false)
        {
            this.Id = id == "" ? Guid.NewGuid().ToString() : id;
            this.Node = node;
            this.Name = name;
            this.ValueType = valueType;
            this.IsIn = isIn;
            this.IsDynamic = isDynamic;
            this.IsReference = valueType == typeof(Node);
            if(value != null)
            {
                if (value.GetType() != valueType) throw new Exception("Invalid type for the value");
                this.Value = value;
            }
        }

        public NodeParameter InstanciateWithValue(object value)
        {
            return new NodeParameter(this.Node, this.Name, this.ValueType, false) { Value = value };
        }

        public bool SetValue(object value)
        {
            //if (!this.ValueType.IsAssignableFrom(value.GetType())) return false;
            this.Value = value;
            return true;
        }

        public object GetValue()
        {
            try
            {
                if (this.IsIn)
                {
                    if (this.Assignments != null)
                    {
                        var v = this.Node.ComputeParameterValue(this, this.Assignments.GetValue());
                        if (this.ValueType == typeof(string) && v == null) v = "";
                        return v;
                    }
                }
                return this.Node.ComputeParameterValue(this, this.Value);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public double GetValueAsDouble()
        {
            if(this.GetValue().GetType() != typeof(double) &&
                this.GetValue().GetType() != typeof(int)
                && this.GetValue().GetType() != typeof(long)
                && this.GetValue().GetType() != typeof(float)) {

                return double.Parse(this.GetValue().ToString(), CultureInfo.InvariantCulture);
            }
            else
            {
                if(this.GetValue().GetType() != typeof(double))
                {
                    return Convert.ToDouble(this.GetValue());
                }
                else
                {
                    return (double)this.GetValue();
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
