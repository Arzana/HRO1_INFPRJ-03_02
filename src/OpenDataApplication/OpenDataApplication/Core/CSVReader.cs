namespace OpenDataApplication.Core
{
    using Mentula.Utilities.Core;
    using Mentula.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization;

    public static class CSVReader
    {
        private static int curLine;
        private static string[] curValues;
        private static CultureInfo usenInfo = CultureInfo.CreateSpecificCulture("us-en");

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="Station"/> from a specified file.
        /// </summary>
        /// <param name="path"> A absolute or relative path to the file. </param>
        /// <returns> A list of readable stations in the file. </returns>
        /// <exception cref="LoggedException"> The file was not a .csv file. </exception>
        public static List<Station> GetStationsFromFile(string path)
        {
            List<Station> result = new List<Station>();
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


                        Station cur;
                        if (ReadStation(out cur)) result.Add(cur);
                        else Log.Error(nameof(CSVReader), $"Unable to read station at line {curLine}, station skipped");
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

        private static bool ReadStation(out Station value)
        {
            bool parsingFailed = false;
            short id;
            int uic;
            StationType type;
            double geo_lat;
            double geo_lng;

            if (!TryParseId(out id)) parsingFailed = true;
            if (!TryParseUIC(out uic)) parsingFailed = true;
            if (!TryParseStationType(out type)) parsingFailed = true;
            if (!TryParseGeoLat(out geo_lat)) parsingFailed = true;
            if (!TryParseGeoLng(out geo_lng)) parsingFailed = true;

            if (parsingFailed)
            {
                value = null;
                return false;
            }

            SerializationInfo info = new SerializationInfo(typeof(Station), new FormatterConverter());
            info.AddValue("id", id);
            info.AddValue("code", curValues[1].ToUpper());
            info.AddValue("uic", uic);
            info.AddValue("naam", curValues[3]);
            info.AddValue("middel_naam", curValues[4]);
            info.AddValue("korte_naam", curValues[5]);
            info.AddValue("friendly", curValues[6]);
            info.AddValue("land", curValues[7].ToUpper());
            info.AddValue("type", type);
            info.AddValue("geo_lat", geo_lat);
            info.AddValue("geo_lng", geo_lng);

            value = new Station(info, new StreamingContext());
            return true;
        }

        private static bool TryParseId(out short value)
        {
            bool result = short.TryParse(curValues[0], out value);
            if (!result) LogLineWarning("Id (Int16)");
            return result;
        }

        private static bool TryParseUIC(out int value)
        {
            bool result = int.TryParse(curValues[2], out value);
            if (!result) LogLineWarning("UIC (Int32)");
            return result;
        }

        private static bool TryParseStationType(out StationType value)
        {
            bool result = Enum.TryParse(curValues[8], out value);
            if (!result) LogLineWarning("Type (StationType)");
            return result;
        }

        private static bool TryParseGeoLat(out double value)
        {
            bool result = double.TryParse(curValues[9], NumberStyles.AllowDecimalPoint, usenInfo, out value);
            if (!result) LogLineWarning("Geo_Lat (Float)");
            return result;
        }

        private static bool TryParseGeoLng(out double value)
        {
            bool result = double.TryParse(curValues[10], NumberStyles.AllowDecimalPoint, usenInfo, out value);
            if (!result) LogLineWarning("Geo_Lng (Float)");
            return result;
        }

        private static void LogLineWarning(string collumn)
        {
            Log.Warning(nameof(CSVReader), $"Could not read {collumn} in line {curLine}");
        }
    }
}