using System;
using System.Collections.Generic;
using Chess.Game.Board;
using Chess.Game.Pieces;
using osu.Framework.Graphics.Primitives;


namespace Chess.Game.Manager
{
    public class GameManager
    {
        private bool whiteMove = true;

        private static GameManager instance = null;
        public static GameManager Instance
        {
            get
            {
                instance ??= new GameManager();
                return instance;
            }
        }

        public PieceColour GetMoveColour()
        {
            return whiteMove ? PieceColour.White : PieceColour.Black;
        }

        public void ToogleMove()
        {
            whiteMove = !whiteMove;
        }

        public bool IsKingInCheck(PieceBase[] board, PieceColour kingColour)
        {
            Vector2I kingPos = new Vector2I(-1, -1);
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null &&  board[i].Type == PieceType.King && board[i].Colour == kingColour)
                {
                    kingPos = new Vector2I(i % ChessBoardGlobals.BOARD_SIZE, i / ChessBoardGlobals.BOARD_SIZE);
                    break;
                }
            }

            foreach (var piece in board)
            {
                if (piece == null || piece.Colour == kingColour)
                    continue;

                Vector2I from = new Vector2I(Array.IndexOf(board, piece) % ChessBoardGlobals.BOARD_SIZE, Array.IndexOf(board, piece) / ChessBoardGlobals.BOARD_SIZE);

                List<Vector2I> moves = piece.GenerateMoves(board, from);
                if (moves.Contains(kingPos))
                    return true;
            }

            return false;
        }
    }
}