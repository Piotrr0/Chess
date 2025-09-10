using osuTK;
using osu.Framework.Graphics.Primitives;
using System;
using System.Collections.Generic;

namespace Chess.Game.Pieces.Positions
{
    public static class PieceInitialPositions
    {
        public static readonly Vector2I WHITE_ROOK_1 = new Vector2I(0, 0);
        public static readonly Vector2I WHITE_KNIGHT_1 = new Vector2I(1, 0);
        public static readonly Vector2I WHITE_BISHOP_1 = new Vector2I(2, 0);
        public static readonly Vector2I WHITE_QUEEN = new Vector2I(3, 0);
        public static readonly Vector2I WHITE_KING = new Vector2I(4, 0);
        public static readonly Vector2I WHITE_BISHOP_2 = new Vector2I(5, 0);
        public static readonly Vector2I WHITE_KNIGHT_2 = new Vector2I(6, 0);
        public static readonly Vector2I WHITE_ROOK_2 = new Vector2I(7, 0);

        public static readonly Vector2I[] WHITE_PAWNS =
        {
            new Vector2I(0, 1), new Vector2I(1, 1), new Vector2I(2, 1), new Vector2I(3, 1),
            new Vector2I(4, 1), new Vector2I(5, 1), new Vector2I(6, 1), new Vector2I(7, 1)
        };

        public static readonly Vector2I BLACK_ROOK_1 = new Vector2I(0, 7);
        public static readonly Vector2I BLACK_KNIGHT_1 = new Vector2I(1, 7);
        public static readonly Vector2I BLACK_BISHOP_1 = new Vector2I(2, 7);
        public static readonly Vector2I BLACK_QUEEN = new Vector2I(3, 7);
        public static readonly Vector2I BLACK_KING = new Vector2I(4, 7);
        public static readonly Vector2I BLACK_BISHOP_2 = new Vector2I(5, 7);
        public static readonly Vector2I BLACK_KNIGHT_2 = new Vector2I(6, 7);
        public static readonly Vector2I BLACK_ROOK_2 = new Vector2I(7, 7);

        public static readonly Vector2I[] BLACK_PAWNS =
        {
            new Vector2I(0, 6), new Vector2I(1, 6), new Vector2I(2, 6), new Vector2I(3, 6),
            new Vector2I(4, 6), new Vector2I(5, 6), new Vector2I(6, 6), new Vector2I(7, 6)
        };

        public static IEnumerable<(Type PieceType, PieceColour Colour, Vector2I Position)> GetAllPieces()
        {
            foreach (var pos in WHITE_PAWNS)
                yield return (typeof(Pawn), PieceColour.White, pos);
            foreach (var pos in BLACK_PAWNS)
                yield return (typeof(Pawn), PieceColour.Black, pos);

            yield return (typeof(Rook), PieceColour.White, WHITE_ROOK_1);
            yield return (typeof(Rook), PieceColour.White, WHITE_ROOK_2);
            yield return (typeof(Rook), PieceColour.Black, BLACK_ROOK_1);
            yield return (typeof(Rook), PieceColour.Black, BLACK_ROOK_2);

            yield return (typeof(Knight), PieceColour.White, WHITE_KNIGHT_1);
            yield return (typeof(Knight), PieceColour.White, WHITE_KNIGHT_2);
            yield return (typeof(Knight), PieceColour.Black, BLACK_KNIGHT_1);
            yield return (typeof(Knight), PieceColour.Black, BLACK_KNIGHT_2);

            yield return (typeof(Bishop), PieceColour.White, WHITE_BISHOP_1);
            yield return (typeof(Bishop), PieceColour.White, WHITE_BISHOP_2);
            yield return (typeof(Bishop), PieceColour.Black, BLACK_BISHOP_1);
            yield return (typeof(Bishop), PieceColour.Black, BLACK_BISHOP_2);

            yield return (typeof(Queen), PieceColour.White, WHITE_QUEEN);
            yield return (typeof(Queen), PieceColour.Black, BLACK_QUEEN);

            yield return (typeof(King), PieceColour.White, WHITE_KING);
            yield return (typeof(King), PieceColour.Black, BLACK_KING);
        }
    }
}