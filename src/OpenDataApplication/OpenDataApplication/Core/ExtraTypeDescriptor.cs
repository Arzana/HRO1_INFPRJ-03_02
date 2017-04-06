namespace OpenDataApplication.Core
{
    using Mentula.Utilities.Logging;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Adds extra functionality for adding and getting <see cref="TypeConverter"/>.
    /// </summary>
    public static class ExtraTypeDescriptor
    {
        static ExtraTypeDescriptor()
        {
            AssignTypeConverter<StationType, EnumTypeConverter<StationType>>();
        }

        /// <summary>
        /// Gets a specified <see cref="TypeConverter"/> that can convert from <see cref="string"/>.
        /// </summary>
        /// <param name="t"> The type to convert to. </param>
        /// <param name="tc"> The typeconverter to use. </param>
        /// <returns> Whether a suitable <see cref="TypeConverter"/> has been found. </returns>
        public static bool GetFromString(Type t, out TypeConverter tc)
        {
            tc = TypeDescriptor.GetConverter(t);

            if (IsNullOrNonString(tc))
            {
                Log.Error(nameof(TypeDescriptor), $"Cannot convert string to type {t.Name}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a <see cref="TypeConverter"/> to the global pool.
        /// </summary>
        /// <typeparam name="Type"> The type to convert to. </typeparam>
        /// <typeparam name="TConverterType"> The <see cref="TypeConverter"/> to use. </typeparam>
        private static void AssignTypeConverter<Type, TConverterType>()
        {
            TypeDescriptor.AddAttributes(typeof(Type), new TypeConverterAttribute(typeof(TConverterType)));
        }

        private static bool IsNullOrNonString(TypeConverter converter)
        {
            return converter == null || !converter.CanConvertFrom(typeof(string));
        }
    }
}
