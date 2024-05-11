using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ScreenLocker.Common.Images
{
    public class ImageProcessor
    {
        public static Bitmap TakeScreenShot()
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            //printscreen.Save(@"C:\printscreen.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return printscreen;
        }
        public static string ConvertImageToString(Bitmap Image)
        {
            //Bitmap bImage = newImage;  // Your Bitmap Image
            System.IO.MemoryStream ms = new MemoryStream();
            Image.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage); // Get Base64
        }
    }
}
