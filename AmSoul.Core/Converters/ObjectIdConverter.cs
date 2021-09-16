using MongoDB.Bson;
using System;
using System.ComponentModel;
using System.Globalization;

namespace AmSoul.Core.Converters
{
    public class ObjectIdConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) => value is string @string ? MongoDB.Bson.ObjectId.Parse(@string) : base.ConvertFrom(context, culture, value);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) => destinationType == typeof(string)
                ? ((ObjectId)value).ToString()
                : base.ConvertTo(context, culture, value, destinationType);
    }
}
