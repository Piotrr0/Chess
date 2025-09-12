using System;
using System.Collections.Generic;
using Chess.Game.Board;
using Chess.Game.Pieces;
using osu.Framework.Graphics.Primitives;


namespace Chess.Game.Manager
{
    public class GameManager
    {
        public Action<PieceColour /* Check Piece Colour */> OnCheck;
        public Action<PieceColour /* CheckMate Piece Colour */> OnCheckmate;
        public Action<PieceColour /* Stalemate Piece Colour */> OnStalemate;

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
            Vector2I kingPos = findKingPosition(board, kingColour);

            for (int i = 0; i < board.Length; i++)
            {
                PieceBase piece = board[i];
                if (piece == null || piece.Colour == kingColour)
                    continue;

                Vector2I piecePos = new Vector2I(i % ChessBoardGlobals.BOARD_SIZE, i / ChessBoardGlobals.BOARD_SIZE);
                List<Vector2I> moves = piece.GenerateMoves(board, piecePos);

                if (moves.Contains(kingPos))
                {
                    OnCheck?.Invoke(kingColour);
                    return true;
                }
            }

            return false;
        }

        private Vector2I findKingPosition(PieceBase[] board, PieceColour kingColour)
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != null && board[i].Type == PieceType.King && board[i].Colour == kingColour)
                {
                    return new Vector2I(i % ChessBoardGlobals.BOARD_SIZE, i / ChessBoardGlobals.BOARD_SIZE);
                }
            }
            return new Vector2I(-1, -1);
        }

        public List<Vector2I> FilterLegalMoves(PieceBase[] board, Vector2I from, List<Vector2I> candidateMoves)
        {
            List<Vector2I> legalMoves = new List<Vector2I>();
            PieceBase movingPiece = board[from.Y * ChessBoardGlobals.BOARD_SIZE + from.X];

            foreach (Vector2I move in candidateMoves)
            {
                int fromIndex = from.Y * ChessBoardGlobals.BOARD_SIZE + from.X;
                int toIndex = move.Y * ChessBoardGlobals.BOARD_SIZE + move.X;
                PieceBase capturedPiece = board[toIndex];
                
                board[toIndex] = movingPiece;
                board[fromIndex] = null;

                bool wouldBeInCheck = IsKingInCheck(board, movingPiece.Colour);

                board[fromIndex] = movingPiece;
                board[toIndex] = capturedPiece;

                if (!wouldBeInCheck)
                {
                    legalMoves.Add(move);
                }
            }

            return legalMoves;
        }

        public bool HasLegalMoves(PieceBase[] board, PieceColour playerColour)
        {
            for (int i = 0; i < board.Length; i++)
            {
                PieceBase piece = board[i];
                if (piece == null || piece.Colour != playerColour)
                    continue;

                Vector2I piecePos = new Vector2I(i % ChessBoardGlobals.BOARD_SIZE, i / ChessBoardGlobals.BOARD_SIZE);
                List<Vector2I> candidateMoves = piece.GenerateMoves(board, piecePos);
                List<Vector2I> legalMoves = FilterLegalMoves(board, piecePos, candidateMoves);

                if (legalMoves.Count > 0)
                    return true;
            }
            return false;
        }

        public bool IsCheckmate(PieceBase[] board, PieceColour playerColour)
        {
            bool isCheckmate = IsKingInCheck(board, playerColour) && !HasLegalMoves(board, playerColour);
            if (isCheckmate)
            {
                OnCheckmate?.Invoke(playerColour);
                return true;
            }
            return false;
        }

        public bool IsStalemate(PieceBase[] board, PieceColour playerColour)
        {
            bool isStalemate = !IsKingInCheck(board, playerColour) && !HasLegalMoves(board, playerColour);
            if (isStalemate)
            {
                OnStalemate?.Invoke(playerColour);
                return true;
            }
            return false;
        }
    }
}