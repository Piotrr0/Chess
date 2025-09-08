using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace Chess.Game.Board
{
    public partial class ChessBoard : CompositeDrawable
    {
        private Container board = null;
        private const int BOARD_SIZE = 8;
        private const float SQUARE_SIZE = 128f;
        private readonly Colour4 light = Colour4.LightGray;
        private readonly Colour4 dark = Color4.SeaGreen;

        public ChessBoard()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            board = new Container
            {
                Size = new Vector2(BOARD_SIZE * SQUARE_SIZE, BOARD_SIZE * SQUARE_SIZE),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            createBoard();
            AddInternal(board);
        }

        private void createBoard()
        {
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    bool isDark = (row + col) % 2 == 1;

                    var square = new Box
                    {
                        Size = new Vector2(SQUARE_SIZE, SQUARE_SIZE),
                        Position = new Vector2(col * SQUARE_SIZE, row * SQUARE_SIZE),
                        Colour = isDark ? dark : light
                    };

                    board.Add(square);
                }
            }
        }
    }
}
