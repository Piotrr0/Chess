using System;
using Chess.Game.Pieces;


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
    }
}