using System;

namespace Samples.Animation
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (AnimationGame game = new AnimationGame())
            {
                game.Run();
            }
        }
    }
}

