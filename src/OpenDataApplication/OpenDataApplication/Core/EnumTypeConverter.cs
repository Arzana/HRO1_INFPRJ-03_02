namespace OpenDataApplication.Core
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Defines a converter from string to a specified enum type.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public sealed class EnumTypeConverter<TEnum> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (CanConvertFrom(value.GetType())) return Enum.Parse(typeof(TEnum), (string)value, true);
            else return base.ConvertFrom(context, culture, value);
        }
    }
}
