using System;

namespace Destiny
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Destiny game = new Destiny())
            {
                var a = 1;
                game.Run();
            }
        }
    }
#endif
}

