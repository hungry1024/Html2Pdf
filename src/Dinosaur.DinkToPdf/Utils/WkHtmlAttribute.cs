using System;

namespace Dinosaur.DinkToPdf
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class WkHtmlAttribute : Attribute
    {
        public string Name { get; private set; }

        public WkHtmlAttribute(string name)
        {
            Name = name;
        }
    }
}
