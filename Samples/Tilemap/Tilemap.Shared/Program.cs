using System;

namespace Samples.Tilemaps
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (TilemapGame game = new TilemapGame())
            {
                game.Run();
            }
        }
    }
}

