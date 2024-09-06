using System;

namespace Samples.FXAA
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (FXAAGame game = new FXAAGame())
            {
                game.Run();
            }
        }
    }
}

