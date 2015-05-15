using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace Packing_Net.Classes
{
    /// <summary>
    /// 
    /// </summary>
   public static class ImageFromUrl
    {
       public static void GetImageFrom(System.Windows.Controls.Image SKUImage, String URLPath)
       {
           try
           {
               var image = new BitmapImage();
               int BytesToRead = 100;

               WebRequest request = WebRequest.Create(new Uri(URLPath , UriKind.Absolute));
               request.Timeout = -1;
               WebResponse response = request.GetResponse();
               Stream responseStream = response.GetResponseStream();
               BinaryReader reader = new BinaryReader(responseStream);
               MemoryStream memoryStream = new MemoryStream();

               byte[] bytebuffer = new byte[BytesToRead];
               int bytesRead = reader.Read(bytebuffer, 0, BytesToRead);

               while (bytesRead > 0)
               {
                   memoryStream.Write(bytebuffer, 0, bytesRead);
                   bytesRead = reader.Read(bytebuffer, 0, BytesToRead);
               }

               image.BeginInit();
               memoryStream.Seek(0, SeekOrigin.Begin);

               image.StreamSource = memoryStream;
               image.EndInit();

               SKUImage.Source = image;

           }
           catch (Exception)
           {
               
               
           }
       }
    }
}
