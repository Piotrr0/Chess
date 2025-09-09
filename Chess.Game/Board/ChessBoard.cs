using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Primitives;
using osuTK;
using osuTK.Graphics;
using Chess.Game.Pieces.Positions;
using Chess.Game.Pieces;
using System;

namespace Chess.Game.Board
{
    public partial class ChessBoard : CompositeDrawable
    {
        private Container boardContainer = null;
        private readonly Colour4 light = Colour4.LightGray;
        private readonly Colour4 dark = Color4.SeaGreen;

        private readonly int boardSize = ChessBoardGlobals.BOARD_SIZE;
        private readonly float squareSize = ChessBoardGlobals.SQUARE_SIZE;

        private PieceBase[] board = new PieceBase[8 * 8];
        public PieceBase SelectedPiece { get; set; }

        public ChessBoard()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            boardContainer = new Container
            {
                Size = new Vector2(boardSize * squareSize, boardSize * squareSize),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            createBoard();
            addInitialPieces();
            AddInternal(boardContainer);
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

                    boardContainer.Add(square);
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

        public Vector2I CalculateIndiciesForPosition(Vector2 screenPosition)
        {
            Vector2 boardTopLeft = boardContainer.ScreenSpaceDrawQuad.TopLeft;
            Vector2 boardTopRight = boardContainer.ScreenSpaceDrawQuad.TopRight;
            Vector2 boardBottomLeft = boardContainer.ScreenSpaceDrawQuad.BottomLeft;

            Vector2 squareSizeOnScreen = new Vector2(
                (boardTopRight.X - boardTopLeft.X) / boardSize,
                (boardBottomLeft.Y - boardTopLeft.Y) / boardSize
            );

            Vector2 relativePosition = screenPosition - boardTopLeft;
            Vector2 indices = new Vector2(relativePosition.X / squareSizeOnScreen.X, relativePosition.Y / squareSizeOnScreen.Y);

            int col = Math.Clamp((int)indices.X, 0, boardSize - 1);
            int row = Math.Clamp((int)indices.Y, 0, boardSize - 1);

            return new Vector2I(col, row);
        }

        private void addInitialPieces()
        {
            foreach (var (pieceType, colour, position) in PieceInitialPositions.GetAllPieces())
            {
                PieceBase piece = (PieceBase)Activator.CreateInstance(pieceType, colour);
                piece.Position = CalculatePiecePosition(position);

                piece.OnPieceDropped += handlePieceDropped;

                boardContainer.Add(piece);
                board[position.Y * boardSize + position.X] = piece;
            }
        }

        private void handlePieceDropped(PieceBase piece, Vector2 screenPosition)
        {
            Vector2I target = CalculateIndiciesForPosition(screenPosition);
            Vector2I current = CalculateIndiciesForPosition(piece.Position);

            board[target.Y * boardSize + target.X] = piece;
            board[current.Y * boardSize + current.X] = null;

            piece.Position = CalculatePiecePosition(new Vector2(target.X, target.Y));
        }
    }
}