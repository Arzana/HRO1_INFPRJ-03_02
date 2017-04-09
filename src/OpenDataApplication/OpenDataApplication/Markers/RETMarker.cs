namespace OpenDataApplication.Markers
{
    using Core;
    using Core.DataTypes;
    using System.Drawing;

    /// <summary>
    /// Defines a marker for all RET stops.
    /// </summary>
    public sealed class RETMarker : CustomMarker
    {
        private static readonly Image busImg = LoadImage("bus_icon.png");
        private static readonly Image metroImg = LoadImage("ret_icon.png");

        /// <summary>
        /// Initializes a new instance of the <see cref="RETMarker"/> class from a specified stop.
        /// </summary>
        /// <param name="stop"> The stop to use for initializing. </param>
        public RETMarker(Stop stop)
            : base(stop.Position, SelectIcon(stop.Type))
        { }

        private static Image SelectIcon(StopType type)
        {
            switch (type)
            {
                case StopType.Bus:
                    return busImg;
                case StopType.Metro:
                    return metroImg;
                case StopType.Tram:
                    return null;    // TODO: Add tram icon.
                case StopType.Ferry:
                    return null;    // TODO: Add ferry icon.
                case StopType.Undefined:
                default:
                    return null;
            }
        }
    }
}
