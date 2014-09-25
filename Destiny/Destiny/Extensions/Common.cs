using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Extensions
{
    public class ExceptionHelper
    {
        public static Exception Throw(Exception e, string message, params object[] p)
        {
            return new Exception(string.Format(message, p), e);
        }

        public static Exception Throw(string message, params object[] p)
        {
            return new Exception(string.Format(message, p), null);
        }
    }
}
