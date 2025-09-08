using osu.Framework.Platform;
using osu.Framework;
using Chess.Game;

namespace Chess.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Chess"))
            using (osu.Framework.Game game = new ChessGame())
                host.Run(game);
        }
    }
}
