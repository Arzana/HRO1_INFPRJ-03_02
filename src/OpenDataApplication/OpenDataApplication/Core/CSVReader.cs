namespace OpenDataApplication.Core
{
    using DataTypes;
    using DeJong.Utilities.Core;
    using DeJong.Utilities.Logging;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    public static class CSVReader
    {
        private static int curLine;                         // The current line in the .csv file, used for logging line errors.

        private static FileCache cache_file;                // The cache for the file specific values.
        private static ReflectionCache cache_refl;          // The cache for the value serialization.

        static CSVReader()
        {
            // The application needs to run in english or united states culture in order for the float and double values to convert properly.
            Application.CurrentCulture = CultureInfo.CreateSpecificCulture("en-us");
            cache_file = new FileCache();
            cache_refl = new ReflectionCache();
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="Station"/> from a specified file.
        /// </summary>
        /// <param name="path"> A absolute or relative path to the file. </param>
        /// <returns> A list of readable stations in the file. </returns>
        /// <exception cref="LoggedException"> The file was not a .csv file. </exception>
        public static List<Station> GetStationsFromFile(string path, bool overrideCache = false)
        {
            return GetData<Station>(path, overrideCache);
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="Stop"/> from a specified file.
        /// </summary>
        /// <param name="path"> A absolute or relative path to the file. </param>
        /// <returns> A list of readable stops in the file. </returns>
        /// <exception cref="LoggedException"> The file was not a .csv file. </exception>
        public static List<Stop> GetStopsFromFile(string path, bool overrideCache = false)
        {
            return GetData<Stop>(path, overrideCache);
        }

        /// <summary>
        /// Clears the stations and stops from the cache.
        /// </summary>
        public static void ClearCache()
        {
            cache_file.Clear();
            cache_refl.Clear();
        }

        // Gets the data from either the file or from cache.
        private static List<T> GetData<T>(string path, bool overrideCache)
            where T : ISerializable, new()
        {
            List<T> result;

            if (overrideCache || !cache_file.TryGetPool(out result))
            {
                result = ReadTypeFromFile<T>(path);
                cache_file.AddPool(result, overrideCache);
            }

            return result;
        }

        /// <summary>
        /// Reads specified types from a specified file.
        /// </summary>
        /// <typeparam name="T"> The type attempt to serialize to. </typeparam>
        /// <param name="fileName"> A absolute or relative path to the file. </param>
        /// <param name="ctor"> A function to user as the types constructor. </param>
        /// <returns> A list of serialized objects. </returns>
        private static List<T> ReadTypeFromFile<T>(string fileName)
            where T : ISerializable, new()
        {
            fileName = $"{Settings.Default.DataDirectory}{fileName}";                   // Get the full path to the file.
            List<T> result = new List<T>();
            curLine = 0;

            LoggedException.RaiseIf(!fileName.EndsWith(".csv"), nameof(CSVReader),      // Raise an exception if the files isn't a .csv file.
                $"Cannot open file with extension {Path.GetExtension(fileName)}, supply .csv file");

            try
            {
                using (StreamReader sr = new StreamReader(fileName))                    // Open a read stream to the file.
                {
                    Log.Verbose(nameof(CSVReader), $"Started parsing file: '{Path.GetFileName(fileName)}'");

                    string line;
                    while ((line = sr.ReadLine()) != null)                              // Read all lines.
                    {
                        ++curLine;                                                      // Increment the current line.

                        T cur;
                        if (DeserializeType(line, out cur)) result.Add(cur);            // Deserialzie a type from the current line.
                    }

                    Log.Info(nameof(CSVReader), $"File contains {result.Count} readable entries");
                }
            }
            catch (Exception e)
            {
                Log.Fatal(nameof(CSVReader), new FileLoadException($"An unhandled exception was raised during the processing of file: '{Path.GetFullPath(fileName)}'", e));
            }

            Log.Verbose(nameof(CSVReader), $"Finished parsing file: '{Path.GetFileName(fileName)}'");
            return result;
        }

        /// <summary>
        /// Deserializes a line to a specified type.
        /// </summary>
        /// <typeparam name="T"> The type to desirialize to. </typeparam>
        /// <param name="line"> The line to user as the source. </param>
        /// <param name="ctor"> A function to user as the types constructor. </param>
        /// <param name="value"> The result value of the deserialization (default(T) if failed). </param>
        /// <returns> Whether the deserialization has succeeded. </returns>
        private static bool DeserializeType<T>(string line, out T value)
            where T : ISerializable, new()
        {
            value = default(T);

            try
            {
                SerializationInfo objInfo = cache_refl.GetInfo<T>();                                // Get a container for the objects types to serialize.
                SerializationInfo resultInfo = cache_refl.GetEmptyInfo<T>();                        // Get a container for the types read from the file.

                string[] rawValues = line.Split(';');                                               // Splits the line so we only have the underlying values.
                if (rawValues.Length != objInfo.MemberCount) return false;                          // Check if the line contains enough members to populate the type.

                int i = 0;
                bool failed = false;
                foreach (SerializationEntry entry in objInfo)                                       // Loop through all members to convert and add their values.
                {
                    // Floats and doubles need to be converted to the correct culture.
                    if (entry.ObjectType == typeof(double) || entry.ObjectType == typeof(float)) rawValues[i] = rawValues[i].Replace(',', '.');
                    try
                    {
                        TypeConverter converter = null; // Attempt to get a type converter for the current type.
                        failed = failed || !ExtraTypeDescriptor.GetFromString(entry.ObjectType, out converter);
                        if (!failed) resultInfo.AddValue(entry.Name, converter.ConvertFromString(rawValues[i++]));
                    }
                    catch (Exception)
                    {
                        failed = true;
                        Log.Warning(nameof(CSVReader), $"Unable to read {entry.Name} at line {curLine} ({(curLine == 1 ? "possible header line" : $"cannot change {rawValues[i - 1]} into {entry.ObjectType}")})");
                    }
                }

                if (failed) return false;                                                           // If the conversion failed return false.
                // Call the constructor for the sepcified type with the converted values.
                value = (T)cache_refl.GetCtor(typeof(T)).Invoke(new object[] { resultInfo, ReflectionCache.Context });
                return true;
            }
            catch (Exception)
            {
                Log.Error(nameof(CSVReader), $"Unable to read {typeof(T).Name} at line {curLine}, station skipped");
                return false;
            }
        }
    }
}