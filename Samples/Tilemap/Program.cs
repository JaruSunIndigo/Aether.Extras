using System;

namespace Samples.Tilemaps
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (SampleGame game = new SampleGame())
            {
                game.Run();
            }
        }
    }
}

