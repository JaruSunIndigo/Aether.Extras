using System;

namespace Samples.Atlas
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (AtlasGame game = new AtlasGame())
            {
                game.Run();
            }
        }
    }
}

