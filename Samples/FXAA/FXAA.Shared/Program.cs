using System;

namespace Samples.FXAA
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

