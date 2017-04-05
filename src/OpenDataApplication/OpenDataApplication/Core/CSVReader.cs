namespace OpenDataApplication.Core
{
    using GMap.NET;
    using Mentula.Utilities.Core;
    using Mentula.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public static class CSVReader
    {
        private static int curLine;
        private static string[] curValues;
        private static CultureInfo usenInfo = CultureInfo.CreateSpecificCulture("us-en");

        public static List<Station> GetStationsFromFile(string path)
        {
            List<Station> result = new List<Station>();
            curLine = 0;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++curLine;
                        curValues = line.Split(';');

                        Station cur;
                        if (ReadStation(out cur)) result.Add(cur);
                        else Log.Error(nameof(CSVReader), $"Unable to read station at line {curLine}, station skipped");
                    }
                }
            }
            catch (Exception e)
            {
                LoggedException.Raise(nameof(CSVReader), $"Un unhandled exception was raised during the processing of file: {path}", e);
            }

            return result;
        }

        private static bool ReadStation(out Station value)
        {
            value = new Station();

            short id;
            int uic;
            StationType type;
            double geo_lat;
            double geo_lng;

            if (!TryParseId(out id)) return false;
            if (!TryParseUIC(out uic)) return false;
            if (!TryParseStationType(out type)) return false;
            if (!TryParseGeoLat(out geo_lat)) return false;
            if (!TryParseGeoLng(out geo_lng)) return false;

            value.Id = id;
            value.Code = curValues[1].ToUpper();
            value.UIC = uic;
            value.FullName = curValues[3];
            value.MiddleName = curValues[4];
            value.ShortName = curValues[5];
            value.FriendlyName = curValues[6];
            value.CountryCode = curValues[7].ToUpper();
            value.Type = type;
            value.Position = new GPoint((long)(geo_lat * 1000000.0f), (long)(geo_lng * 1000000.0f));

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
            bool result = double.TryParse(curValues[9], out value);
            if (!result) LogLineWarning("Geo_Lat (Float)");
            return result;
        }

        private static bool TryParseGeoLng(out double value)
        {
            bool result = double.TryParse(curValues[10], out value);
            if (!result) LogLineWarning("Geo_Lng (Float)");
            return result;
        }

        private static void LogLineWarning(string collumn)
        {
            Log.Warning(nameof(CSVReader), $"Could not read {collumn} in line {curLine}");
        }
    }
}