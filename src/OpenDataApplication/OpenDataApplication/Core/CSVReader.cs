namespace OpenDataApplication.Core
{
    using Mentula.Utilities.Core;
    using Mentula.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Serialization;

    public static class CSVReader
    {
        private static int curLine;
        private static string[] curValues;

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="Station"/> from a specified file.
        /// </summary>
        /// <param name="path"> A absolute or relative path to the file. </param>
        /// <returns> A list of readable stations in the file. </returns>
        /// <exception cref="LoggedException"> The file was not a .csv file. </exception>
        public static List<Station> GetStationsFromFile(string path)
        {
            return ReadTypeFromFile(path, (i, c) => new Station(i, c));
        }

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="Stop"/> from a specified file.
        /// </summary>
        /// <param name="path"> A absolute or relative path to the file. </param>
        /// <returns> A list of readable stops in the file. </returns>
        /// <exception cref="LoggedException"> The file was not a .csv file. </exception>
        public static List<Stop> GetStopsFromFile(string path)
        {
            return ReadTypeFromFile(path, (i, c) => new Stop(i, c));
        }

        /// <summary>
        /// Reads specified types from a specified file.
        /// </summary>
        /// <typeparam name="T"> The type attempt to serialize to. </typeparam>
        /// <param name="path"> A absolute or relative path to the file. </param>
        /// <param name="ctor"> A function to user as the types constructor. </param>
        /// <returns> A list of serialized objects. </returns>
        private static List<T> ReadTypeFromFile<T>(string path, Func<SerializationInfo, StreamingContext, T> ctor)
            where T : ISerializable, new()
        {
            List<T> result = new List<T>();
            curLine = 0;

            LoggedException.RaiseIf(!path.EndsWith(".csv"), nameof(CSVReader),
                $"Cannot open file with extension {Path.GetExtension(path)}, supply .csv file");

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Log.Verbose(nameof(CSVReader), $"Started parsing file: {Path.GetFullPath(path)}");

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++curLine;
                        curValues = line.Split(';');

                        T cur;
                        if (DeserializeType(line, ctor, out cur)) result.Add(cur);
                    }

                    Log.Info(nameof(CSVReader), $"File contains {result.Count} readable entries");
                }
            }
            catch (Exception e)
            {
                Log.Fatal(nameof(CSVReader), new FileLoadException($"An unhandled exception was raised during the processing of file: {Path.GetFullPath(path)}", e));
            }

            Log.Verbose(nameof(CSVReader), $"Finished parsing file: {Path.GetFullPath(path)}");
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
        private static bool DeserializeType<T>(string line, Func<SerializationInfo, StreamingContext, T> ctor, out T value)
            where T : ISerializable, new()
        {
            value = default(T);

            try
            {
                StreamingContext context = new StreamingContext();
                FormatterConverter formatConverter = new FormatterConverter();

                SerializationInfo objInfo = new SerializationInfo(typeof(T), formatConverter);
                SerializationInfo resultInfo = new SerializationInfo(typeof(T), formatConverter);
                new T().GetObjectData(objInfo, context);

                string[] rawValues = line.Split(';');
                if (rawValues.Length != objInfo.MemberCount) return false;

                int i = 0;
                bool failed = false;
                foreach (SerializationEntry entry in objInfo)
                {
                    if (entry.ObjectType == typeof(double) || entry.ObjectType == typeof(float)) rawValues[i] = rawValues[i].Replace(',', '.');
                    try
                    {
                        TypeConverter converter = null;
                        failed = failed || !ExtraTypeDescriptor.GetFromString(entry.ObjectType, out converter);
                        if (!failed) resultInfo.AddValue(entry.Name, converter.ConvertFromString(rawValues[i++]));
                    }
                    catch (Exception)
                    {
                        failed = true;
                        Log.Warning(nameof(CSVReader), $"Unable to read {entry.Name} at line {curLine} (cannot change {rawValues[i - 1]} into {entry.ObjectType})");
                    }
                }

                if (failed) return false;

                value = ctor(resultInfo, context);
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