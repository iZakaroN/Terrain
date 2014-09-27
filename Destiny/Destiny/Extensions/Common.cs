using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Extensions
{
    static public class ExceptionHelper
    {
        public static Exception Throw(Exception e, string message, params object[] p)
        {
            return new Exception(string.Format(message, p), e);
        }

        public static Exception Throw(string message, params object[] p)
        {
            return new Exception(string.Format(message, p), null);
        }

		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}
    }
}
