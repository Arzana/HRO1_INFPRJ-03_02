namespace OpenDataApplication.Core
{
    using GMap.NET;
    using GMap.NET.WindowsForms;
    using Mentula.Utilities.Logging;
    using Properties;
    using System;
    using System.Drawing;
    using System.IO;

    /// <summary>
    /// Defines a base class for markers with a custom icon.
    /// </summary>
    public abstract class CustomMarker : GMapMarker
    {
        private Image img;
        private bool validImg;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMarker"/> class with a specified position and image.
        /// </summary>
        /// <param name="pos"> The location of the marker. </param>
        /// <param name="img"> The image to use as marker. </param>
        protected CustomMarker(PointLatLng pos, Image img)
            : base(pos)
        {
            this.img = img;
            if (img != null)
            {
                validImg = true;
                Log.Debug(nameof(CustomMarker), $"Added custom marker at {pos}");
            }
            else Log.Warning(nameof(CustomMarker), $"Added NULL marker at {pos}");
        }

        /// <inheritdoc/>
        public override void OnRender(Graphics g)
        {
            if (validImg) g.DrawImage(img, LocalPosition);
            base.OnRender(g);
        }

        /// <summary>
        /// Loads a specified image from the icon directory.
        /// </summary>
        /// <param name="fileName"> The name of the image. </param>
        /// <returns> An <see cref="Image"/> if the load was successfull; otherwise, <see langword="null"/>. </returns>
        protected static Image LoadImage(string fileName)
        {
            Log.Info(nameof(CustomMarker), $"Loading marker texture: {Path.GetFileNameWithoutExtension(fileName)}");

            try
            {
                return Image.FromFile($"{Settings.Default.IconDirectory}{fileName}");
            }
            catch (Exception e)
            {
                Log.Fatal(nameof(CustomMarker), e);
                return null;
            }
        }
    }
}