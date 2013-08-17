using System;
using System.Drawing;
using System.Net;

namespace WinGrooves.Utils
{
    public static class ImageUtils
    {
        /// <summary>
        /// Retrieves the image at the url.
        /// </summary>
        /// <param name="url">url of image.</param>
        /// <returns>Returns image as bitmap, or null if not available.</returns>
        public static Bitmap BitmapFromUrl(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                var request = WebRequest.Create(url);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null) return null;
                    return (Bitmap) Image.FromStream(stream);
                }
            }
            catch
            {
                //If something goes wrong downloading the Album art
                //Don't worry about it. This is just a nice to have.
                return null;
            }
          
        }

        /// <summary>
        /// Returns a bitmap of the current form.
        /// </summary>
        /// <returns>Returns a bitmap of the current form</returns>
        public static Bitmap ApplicationThumbnail(System.Windows.Forms.Form form)
        {
            Graphics myGraphics = form.CreateGraphics();
            Size s = form.Size;
            Bitmap memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(form.Location.X, form.Location.Y, 0, 0, s);
            return memoryImage;
        }

    }
}
