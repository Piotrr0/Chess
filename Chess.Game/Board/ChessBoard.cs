using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;
using Chess.Game.Pieces.Positions;
using System;
using Chess.Game.Pieces;

namespace Chess.Game.Board
{
    public partial class ChessBoard : CompositeDrawable
    {
        private Container board = null;
        private readonly Colour4 light = Colour4.LightGray;
        private readonly Colour4 dark = Color4.SeaGreen;

        private readonly int boardSize = ChessBoardGlobals.BOARD_SIZE;
        private readonly float squareSize = ChessBoardGlobals.SQUARE_SIZE;

        public ChessBoard()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            board = new Container
            {
                Size = new Vector2(boardSize * squareSize, boardSize * squareSize),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            createBoard();
            addInitialPieces();
            AddInternal(board);
        }

        private void createBoard()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    bool isDark = (row + col) % 2 == 1;

                    var square = new Box
                    {
                        Size = new Vector2(squareSize, squareSize),
                        Position = new Vector2(col * squareSize, row * squareSize),
                        Colour = isDark ? dark : light
                    };

                    board.Add(square);
                }
            }
        }

        /*X - col | Y - row*/
        public Vector2 CalculatePiecePosition(Vector2 position, float padding = 0f)
        {
            float x = position.X * squareSize + squareSize * padding;
            float y = position.Y * squareSize + squareSize * padding;
            return new Vector2(x, y);
        }

        private void addInitialPieces()
        {
            foreach (var (pieceType, colour, position) in PieceInitialPositions.GetAllPieces())
            {
                var piece = (PieceBase)Activator.CreateInstance(pieceType, colour);
                piece.Position = CalculatePiecePosition(position);
                board.Add(piece);
            }
        }
    }
}