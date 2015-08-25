using System;

namespace ZRTS
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ZRTSGame game = new ZRTSGame())
            {
                game.Run();
            }
        }
    }
#endif
}

