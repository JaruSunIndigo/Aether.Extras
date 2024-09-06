using System;

namespace Samples.SLMC
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (SLMCGame game = new SLMCGame())
            {
                game.Run();
            }
        }
    }
}

