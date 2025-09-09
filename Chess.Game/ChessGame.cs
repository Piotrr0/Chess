using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using Chess.Game.Board;
using Chess.Game.Screens;

namespace Chess.Game
{
    public partial class ChessGame : ChessGameBase
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load()
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            screenStack.Push(new GameScreen());
            Add(new ChessBoard());
        }
    }
}
