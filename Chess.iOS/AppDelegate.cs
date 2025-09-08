using osu.Framework.iOS;
using Chess.Game;

namespace Chess.iOS
{
    /// <inheritdoc />
    public class AppDelegate : GameApplicationDelegate
    {
        /// <inheritdoc />
        protected override osu.Framework.Game CreateGame() => new ChessGame();
    }
}
