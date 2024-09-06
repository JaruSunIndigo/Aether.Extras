using System;

namespace Samples.Deferred
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (DeferredGame game = new DeferredGame())
            {
                game.Run();
            }
        }
    }
}

