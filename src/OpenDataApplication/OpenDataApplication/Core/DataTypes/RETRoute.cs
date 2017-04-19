using DeJong.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenDataApplication.Core.DataTypes
{
    public sealed class RETRoute
    {
        public RETType Type { get; set; }
        public int CompanyId { get; set; }
        public int LineNum { get; set; }
        public Direction Direction { get; set; }
        public int RouteNum { get; set; }
        public List<RouteStop> Stops { get; set; }

        public RETRoute()
        {
            Type = RETType.Unknown;
            Stops = new List<RouteStop>();
        }

        internal void SetTypeFromChar(char type)
        {
            switch (type)
            {
                case ('B'):
                    Type = RETType.Bus;
                    break;
                case ('H'):
                    Type = RETType.NightBus;
                    break;
                case ('T'):
                    Type = RETType.Tram;
                    break;
                case ('M'):
                    Type = RETType.Metro;
                    break;
                default:
                    Type = RETType.Unknown;
                    Log.Warning(nameof(RETRoute), $"Type set as unknown type ({type})");
                    break;
            }
        }

        public override string ToString()
        {
            return $"{{{Type} stops at {Stops.Count} stops}}";
        }
    }

    public class BigRouteStop : RouteStop
    {
        public TimeSpan Arrive { get; set; }

        public void SetArriveFromString(string leave)
        {
            Arrive = new TimeSpan(int.Parse(leave.Substring(0, 2)), int.Parse(leave.Substring(2, 2)), 0);
        }
    }

    public class RouteStop
    {
        public string Id { get; set; }
        public TimeSpan Leave { get; set; }

        public void SetLeaveFromString(string leave)
        {
            Leave = new TimeSpan(int.Parse(leave.Substring(0, 2)), int.Parse(leave.Substring(2, 2)), 0);
        }

        public override string ToString()
        {
            return $"{Id}";
        }
    }

    public enum RETType : byte
    {
        Unknown,
        Bus,
        NightBus,
        Tram,
        Metro
    }

    public enum Direction : byte
    {
        Away = 1,
        Back = 2,
        Both = 0
    }
}