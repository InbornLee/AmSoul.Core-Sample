using System;
using System.ComponentModel;

namespace AmSoul.Core.Converters
{
    internal static class TypeConverterResolver
    {
        internal static void RegisterTypeConverter<T, TConverter>() where TConverter : TypeConverter
        {
            Attribute[] attr = new Attribute[1];
            TypeConverterAttribute converterAttribute = new(typeof(TConverter));
            attr[0] = converterAttribute;
            TypeDescriptor.AddAttributes(typeof(T), attr);
        }
    }
}
