using Chess.Game.Pieces;

namespace Chess.Game.Board.Utils
{
    public static class ChessBoardUtils
    {
        public static bool IsInsideBoard(int x, int y) => x >= 0 && x < ChessBoardGlobals.BOARD_SIZE && y >= 0 && y < ChessBoardGlobals.BOARD_SIZE;
        
        public static bool CanMoveTo(PieceBase[] board, int x, int y, PieceColour movingColour)
        {
            if (!IsInsideBoard(x, y))
                return false;

            PieceBase target = board[y * ChessBoardGlobals.BOARD_SIZE + x];
            return target == null || target.Colour != movingColour;
        }
    }
}