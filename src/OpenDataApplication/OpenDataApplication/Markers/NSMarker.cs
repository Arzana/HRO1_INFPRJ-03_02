namespace OpenDataApplication.Markers
{
    using Core;
    using GMap.NET;
    using System.Drawing;

    /// <summary>
    /// Defines a marker for NS stations.
    /// </summary>
    public sealed class NSMarker : CustomMarker
    {
        private static readonly Image nsIcon = LoadImage("ns_icon.png");

        /// <summary>
        /// Initializes a new instance of the <see cref="NSMarker"/> class with a specified position.
        /// </summary>
        /// <param name="pos"> The location of the marker. </param>
        public NSMarker(PointLatLng pos)
            : base(pos, nsIcon)
        { }
    }
}
