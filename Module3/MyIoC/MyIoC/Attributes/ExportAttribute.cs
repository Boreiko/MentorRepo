using System;


namespace MyIoC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportAttribute : Attribute
    {
        public ExportAttribute()
        { }

        public ExportAttribute(Type dependentType)
        {
            this.dependentType = dependentType;
        }

        public Type dependentType { get; set; }
    }
}

