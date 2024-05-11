using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.ScreenTranslating
{
    public class ScreenTranslator
    {
        public ScreenTranslator() 
        {
        
        }
        public static Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
            }

            return screenshot;
        }
        public void StreamScreen()
        {
            TcpClient client = new TcpClient("127.0.0.1", 8080);
            NetworkStream stream = client.GetStream();

            while (true)
            {
                Bitmap screenshot = CaptureScreen();
                MemoryStream memoryStream = new MemoryStream();

                screenshot.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = memoryStream.ToArray();

                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();

                Thread.Sleep(1000); // Delay between each screen capture
            }

            stream.Close();
            client.Close();
        }
    }
}
