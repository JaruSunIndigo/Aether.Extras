using System;

namespace Samples.Animation
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

