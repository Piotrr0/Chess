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
        public Vector2I? EnPassantTarget { get; set; } = null;

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

        public void ClearEnPassant()
        {
            EnPassantTarget = null;
        }

        public bool IsKingInCheck(PieceBase[] board, PieceColour kingColour)
        {
            Vector2I kingPos = findKingPosition(board, kingColour);
            if (kingPos.X == -1) return false;

            PieceColour opponent = kingColour == PieceColour.White ? PieceColour.Black : PieceColour.White;
            bool attacked = IsSquareAttacked(board, kingPos, opponent);

            if (attacked)
                OnCheck?.Invoke(kingColour);

            return attacked;
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

        public bool IsSquareAttacked(PieceBase[] board, Vector2I square, PieceColour byColour)
        {
            for (int i = 0; i < board.Length; i++)
            {
                PieceBase piece = board[i];
                if (piece == null || piece.Colour != byColour)
                    continue;

                Vector2I from = new Vector2I(i % ChessBoardGlobals.BOARD_SIZE, i / ChessBoardGlobals.BOARD_SIZE);

                switch (piece.Type)
                {
                    case PieceType.Pawn:
                    {
                        int dir = (byColour == PieceColour.White) ? 1 : -1;
                        if (from + new Vector2I(1, dir) == square) return true;
                        if (from + new Vector2I(-1, dir) == square) return true;
                    }
                    break;

                    case PieceType.Knight:
                    {
                        Vector2I[] knightOffsets = {
                            new Vector2I( 2,  1), new Vector2I( 2, -1),
                            new Vector2I(-2,  1), new Vector2I(-2, -1),
                            new Vector2I( 1,  2), new Vector2I( 1, -2),
                            new Vector2I(-1,  2), new Vector2I(-1, -2)
                        };

                        foreach (var target in piece.AddMovesFromOffsets(knightOffsets, board, from))
                            if (target == square) return true;
                    }
                    break;

                    case PieceType.Bishop:
                    {
                        Vector2I[] dirs = { new Vector2I(1, 1), new Vector2I(1, -1), new Vector2I(-1, 1), new Vector2I(-1, -1) };
                        foreach (var target in piece.GetMovesInDirections(dirs, board, from))
                            if (target == square) return true;
                    }
                    break;

                    case PieceType.Rook:
                    {
                        Vector2I[] dirs = { new Vector2I(1, 0), new Vector2I(-1, 0), new Vector2I(0, 1), new Vector2I(0, -1) };
                        foreach (var target in piece.GetMovesInDirections(dirs, board, from))
                            if (target == square) return true;
                    }
                    break;

                    case PieceType.Queen:
                    {
                        Vector2I[] dirs = {
                            new Vector2I(1,0), new Vector2I(-1,0), new Vector2I(0,1), new Vector2I(0,-1),
                            new Vector2I(1,1), new Vector2I(1,-1), new Vector2I(-1,1), new Vector2I(-1,-1)
                        };
                        foreach (var target in piece.GetMovesInDirections(dirs, board, from))
                            if (target == square) return true;
                    }
                    break;

                    case PieceType.King:
                    {
                        Vector2I[] dirs = {
                            new Vector2I( 1,  0), new Vector2I(-1,  0),
                            new Vector2I( 0,  1), new Vector2I( 0, -1),
                            new Vector2I( 1,  1), new Vector2I( 1, -1),
                            new Vector2I(-1,  1), new Vector2I(-1, -1)
                        };
                        foreach (var target in piece.AddMovesFromOffsets(dirs, board, from))
                            if (target == square) return true;
                    }
                    break;
                }
            }
            return false;
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