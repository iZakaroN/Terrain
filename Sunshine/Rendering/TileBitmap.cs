using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Sunshine.Rendering
{
    class TileBitmap
    {
        string ResourceID;
        Options Options;
        public BitmapImage Bitmap;
        public TileBitmap(string resourceID, Options options)
        {
            ResourceID = resourceID;
            Options = options;
            Bitmap = LoadBitmapFromResource("Images/"+resourceID, options.TileSize);
        }

        public static BitmapImage LoadBitmapFromResource(string pathInApplication, int width, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication.Substring(1);
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute);
            bitmap.DecodePixelWidth = width;
            bitmap.EndInit();
            return bitmap;
        }


    }
}
