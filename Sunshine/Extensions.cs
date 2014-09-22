using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sunshine
{
    static public class Extensions
    {
        static public List<Rect> Substract(this Rect r1, Rect r2)
        {
            List<Rect> result = new List<Rect>();
            //Left part
            if (r1.Left < r2.Left)
                result.Add(new Rect(r1.Left, r1.Top, Math.Min(r1.Right, r2.Left) - r1.Left, r1.Height));
            //Right part
            if (r1.Right> r2.Right)
                result.Add(new Rect(Math.Max(r1.Left, r2.Right), r1.Top, r1.Right - Math.Max(r1.Left, r2.Right), r1.Height));

            if (r1.Right > r2.Left && r1.Left < r2.Right) //Top and bottom are only valid if they intersect thier verticals
            {
                //Top part
                if (r1.Top < r2.Top)
                    result.Add(new Rect(Math.Max(r1.Left, r2.Left), r1.Top, Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left), Math.Min(r1.Bottom, r2.Top) - r1.Top));
                //Bottom part
                if (r1.Bottom > r2.Bottom)
                    result.Add(new Rect(Math.Max(r1.Left, r2.Left), Math.Max(r1.Top, r2.Bottom), Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left), r1.Bottom - Math.Max(r1.Top, r2.Bottom)));
            }

            return result;
        }
    }
}
