namespace OpenDataApplication.Core
{
    using DataTypes;
    using DeJong.Utilities.Core;
    using DeJong.Utilities.Logging;
    using Properties;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public static class PASReader
    {
        private static int curLine;

        public static List<RETRoute> ReadRoutesFromFile(string fileName)
        {
            fileName = $"{Settings.Default.DataDirectory}{fileName}";
            List<RETRoute> result = new List<RETRoute>();
            curLine = 0;

            LoggedException.RaiseIf(!fileName.EndsWith(".PAS"), nameof(PASReader),
                $"Cannot open file with extension {Path.GetExtension(fileName)}, supply .csv file");

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    Log.Verbose(nameof(CSVReader), $"Started parsing file: '{Path.GetFileName(fileName)}'");
                    RETRoute cur = null;

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ++curLine;

                        try
                        {
                            switch (line[0])
                            {
                                case ('!'):
                                    CheckFormat(line);
                                    break;
                                case ('&'):
                                    CheckValidity(line);
                                    break;
                                case ('#'):
                                    cur = ReadRouteLine(line);
                                    break;
                                case ('-'):
                                    Log.Debug(nameof(PASReader), "Skipped day code line (Useless)");
                                    ReadDayCodeLine(line);
                                    break;
                                case ('>'):
                                    if (cur == null) Log.Error(nameof(PASReader), $"Floating stop at line: {curLine} (Ignored)");
                                    else if (cur.Stops.Count > 0) Log.Error(nameof(PASReader), $"Second start stop at line: {curLine} (Ignored)");
                                    else cur.Stops.Add(ReadStopStartLine(line));
                                    break;
                                case ('.'):
                                    if (cur == null) Log.Error(nameof(PASReader), $"Floating stop at line: {curLine} (Ignored)");
                                    else if (cur.Stops.Count < 1) Log.Error(nameof(PASReader), $"Intermediate stop with no start at line: {curLine} (Ignored)");
                                    else cur.Stops.Add(ReadStopLine(line));
                                    break;
                                case ('+'):
                                    if (cur == null) Log.Error(nameof(PASReader), $"Floating big stop at line: {curLine} (Ignored)");
                                    else if (cur.Stops.Count < 1) Log.Error(nameof(PASReader), $"Intermediate stop with no start at line: {curLine} (Ignored)");
                                    else cur.Stops.Add(ReadStopLine(line));
                                    break;
                                case ('<'):
                                    if (cur == null) Log.Error(nameof(PASReader), $"Floating stop at line: {curLine} (Ignored)");
                                    else if (cur.Stops.Count < 1) Log.Error(nameof(PASReader), $"End stop with no start at line: {curLine} (Ignored)");
                                    else cur.Stops.Add(ReadStopEndLine(line));

                                    result.Add(cur);
                                    cur = null;
                                    break;
                                default:
                                    Log.Warning(nameof(PASReader), $"Unknown line type at line: {curLine}");
                                    break;
                            }
                        }
                        catch(Exception e)
                        {
                            LoggedException.Raise(nameof(PASReader), $"An unhandled excpetion was raised at line: {curLine}", e);
                        }
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

        private static BigRouteStop ReadBigStopLine(string line)
        {
            CheckNullLine(line, "route end line");
            LoggedException.RaiseIf(line[0] != '+' || line[8] != ',' || line.Length != 17, nameof(PASReader), "Invalid route end format");

            BigRouteStop result = new BigRouteStop { Id = line.Substring(1, 6) };
            result.SetLeaveFromString(line.Substring(9, 4));
            result.SetArriveFromString(line.Substring(13, 4));
            return result;
        }

        private static RouteStop ReadStopEndLine(string line)
        {
            CheckNullLine(line, "route end line");
            LoggedException.RaiseIf(line[0] != '<' || line[8] != ',' || line.Length != 13, nameof(PASReader), "Invalid route end format");

            RouteStop result = new RouteStop { Id = line.Substring(1, 6) };
            result.SetLeaveFromString(line.Substring(9, 4));
            return result;
        }

        private static RouteStop ReadStopLine(string line)
        {
            CheckNullLine(line, "route line");
            LoggedException.RaiseIf(line[0] != '.' || line[8] != ',' || line.Length != 13, nameof(PASReader), "Invalid route format");

            RouteStop result = new RouteStop { Id = line.Substring(1, 6) };
            result.SetLeaveFromString(line.Substring(9, 4));
            return result;
        }

        private static RouteStop ReadStopStartLine(string line)
        {
            CheckNullLine(line, "route start line");
            LoggedException.RaiseIf(line[0] != '>' || line[8] != ',' || line.Length != 13, nameof(PASReader), "Invalid route start format");

            RouteStop result = new RouteStop { Id = line.Substring(1, 6) };
            result.SetLeaveFromString(line.Substring(9, 4));
            return result;
        }

        private static int ReadDayCodeLine(string line)
        {
            CheckNullLine(line, "day code");
            LoggedException.RaiseIf(line[0] != '-' || line.Length != 6, nameof(PASReader), "Invalid day code format");

            return int.Parse(line.Substring(1, 5));
        }

        private static RETRoute ReadRouteLine(string line)
        {
            CheckNullLine(line, "route");
            LoggedException.RaiseIf(line[0] != '#' || line[2] != ',' || line[9] != ','
                || line[13] != ',' || line[15] != ',' || line.Length != 21, nameof(PASReader), "Invalid route line format");

            RETRoute result = new RETRoute
            {
                CompanyId = int.Parse(line.Substring(3, 5)),
                LineNum = int.Parse(line.Substring(10, 2)),
                Direction = (Direction)Convert.ToInt32(line[14]),
                RouteNum = int.Parse(line.Substring(16, 4))
            };
            result.SetTypeFromChar(line[1]);

            return result;
        }

        private static void CheckValidity(string line)
        {
            CheckNullLine(line, "validity");
            LoggedException.RaiseIf(line[0] != '&' || line[9] != '-' || line.Length != 18, nameof(PASReader), "Invalid validity format");

            DateTime start = DateTime.ParseExact(line.Substring(1, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(line.Substring(10, 8), "ddMMyyyy", CultureInfo.InvariantCulture);

            if (start > DateTime.Now) Log.Warning(nameof(PASReader), $"Validty is not yet active ({start})");
            if (end < DateTime.Now) Log.Warning(nameof(PASReader), $"Validity is no longer active ({end})");
        }

        private static void CheckFormat(string line)
        {
            CheckNullLine(line, "format");
            LoggedException.RaiseIf(line != "!RETPAS1", nameof(PASReader), "Unknown PAS format");
        }

        private static void CheckNullLine(string line, string type)
        {
            LoggedException.RaiseIf(string.IsNullOrEmpty(line), nameof(PASReader), $"Unable to read {type} line", new NullReferenceException());
        }
    }
}