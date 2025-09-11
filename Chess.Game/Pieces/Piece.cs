using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Graphics.Textures;
using Chess.Game.Pieces.Textures;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Primitives;
using System;
using System.Collections.Generic;
using Chess.Game.Board.Utils;
using Chess.Game.Board;
using Chess.Game.Manager;

namespace Chess.Game.Pieces
{
    public interface IPiece
    {
        PieceType Type { get; }
        PieceColour Colour { get; set; }
        Sprite Sprite { get; }
    }

    public abstract partial class PieceBase : CompositeDrawable, IPiece
    {
        public event Action<PieceBase, Vector2> OnPieceDropped;
        public event Action<PieceBase, Vector2> OnPieceSelected;

        public PieceType Type { get; protected set; } = PieceType.None;
        public PieceColour Colour { get; set; } = PieceColour.None;
        public Sprite Sprite { get; protected set; }

        public bool IsDragging { get; private set; } = false;
        protected TextureStore Textures { get; set; }

        public PieceBase(PieceColour colour)
        {
            Colour = colour;

            Size = new Vector2(Board.ChessBoardGlobals.SQUARE_SIZE);
            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Textures = textures;
            LoadTexture();
        }

        protected abstract void LoadTexture();

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if(Colour == GameManager.Instance.GetMoveColour())

            if (e.Button == osuTK.Input.MouseButton.Left)
            {
                IsDragging = true;
                OnPieceSelected?.Invoke(this, e.ScreenSpaceMousePosition);
                return true;
            }
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            if (IsDragging && e.Button == osuTK.Input.MouseButton.Left)
            {
                IsDragging = false;
                OnPieceDropped?.Invoke(this, e.ScreenSpaceMousePosition);
            }
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (IsDragging)
            {
                Position = Parent.ToLocalSpace(e.ScreenSpaceMousePosition) - Size / 2;
                return true;
            }

            return base.OnMouseMove(e);
        }

        public abstract List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from);

        protected List<Vector2I> GetMovesInDirections(Vector2I[] dirs, PieceBase[] board, Vector2I from)
        {
            List<Vector2I> moves = new List<Vector2I>();
            foreach (Vector2I dir in dirs)
            {
                int x = from.X;
                int y = from.Y;

                while (true)
                {
                    x += dir.X;
                    y += dir.Y;

                    if (ChessBoardUtils.CanMoveTo(board, x, y, Colour))
                        moves.Add(new Vector2I(x, y));
                    else
                        break;
                }
            }

            return moves;
        }

        protected List<Vector2I> AddMovesFromOffsets(Vector2I[] dirs, PieceBase[] board, Vector2I from)
        {
            List<Vector2I> moves = new List<Vector2I>();
            foreach (Vector2I dir in dirs)
            {
                int targetX = from.X + dir.X;
                int targetY = from.Y + dir.Y;

                if (ChessBoardUtils.CanMoveTo(board, targetX, targetY, Colour))
                    moves.Add(new Vector2I(targetX, targetY));
            }
            return moves;
        }
    }

    public partial class Pawn : PieceBase
    {
        public Pawn(PieceColour colour)
            : base(colour)
        {
            Type = PieceType.Pawn;
        }

        protected override void LoadTexture()
        {
            Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Colour == PieceColour.White ? Textures.Get(PieceTextures.WHITE_PAWN) : Textures.Get(PieceTextures.BLACK_PAWN)
            };
            AddInternal(Sprite);
        }

        public override List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from)
        {
            List<Vector2I> moves = new List<Vector2I>();

            int dir = Colour == PieceColour.White ? 1 : -1;
            int targetY = from.Y + dir;

            if (ChessBoardUtils.IsInsideBoard(from.X, targetY) && board[targetY * ChessBoardGlobals.BOARD_SIZE + from.X] == null)
                moves.Add(new Vector2I(from.X, targetY));

            if ((Colour == PieceColour.White && from.Y == 1) || (Colour == PieceColour.Black && from.Y == 6))
            {
                int doubleY = from.Y + dir * 2;
                if (board[doubleY * ChessBoardGlobals.BOARD_SIZE + from.X] == null && board[targetY * ChessBoardGlobals.BOARD_SIZE + from.X] == null)
                    moves.Add(new Vector2I(from.X, doubleY));
            }

            foreach (int dx in new[] { -1, 1 })
            {
                int tx = from.X + dx;
                if (ChessBoardUtils.IsInsideBoard(tx, targetY))
                {
                    var target = board[targetY * ChessBoardGlobals.BOARD_SIZE + tx];
                    if (target != null && target.Colour != Colour)
                        moves.Add(new Vector2I(tx, targetY));
                }
            }

            return moves;
        }
    }

    public partial class Knight : PieceBase
    {
        public Knight(PieceColour colour)
            : base(colour)
        {
            Type = PieceType.Knight;
        }

        protected override void LoadTexture()
        {
            Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Colour == PieceColour.White ? Textures.Get(PieceTextures.WHITE_KNIGHT) : Textures.Get(PieceTextures.BLACK_KNIGHT)
            };
            AddInternal(Sprite);
        }

        public override List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from)
        {
            Vector2I[] dirs = {
                new Vector2I(2, 1),
                new Vector2I(2, -1),
                new Vector2I(-2, 1),
                new Vector2I(-2, -1),
                new Vector2I(1, 2),
                new Vector2I(1, -2),
                new Vector2I(-1, 2),
                new Vector2I(-1, -2)
            };

            return AddMovesFromOffsets(dirs, board, from);
        }
    }

    public partial class Bishop : PieceBase
    {
        public Bishop(PieceColour colour)
            : base(colour)
        {
            Type = PieceType.Bishop;
        }

        protected override void LoadTexture()
        {
            Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Colour == PieceColour.White ? Textures.Get(PieceTextures.WHITE_BISHOP) : Textures.Get(PieceTextures.BLACK_BISHOP)
            };
            AddInternal(Sprite);
        }

        public override List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from)
        {
            Vector2I[] dirs = {
                new Vector2I( 1,  1),
                new Vector2I( 1, -1),
                new Vector2I(-1,  1),
                new Vector2I(-1, -1)
            };

            return GetMovesInDirections(dirs, board, from);
        }
    }

    public partial class Rook : PieceBase
    {
        public Rook(PieceColour colour)
            : base(colour)
        {
            Type = PieceType.Rook;
        }

        protected override void LoadTexture()
        {
            Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Colour == PieceColour.White ? Textures.Get(PieceTextures.WHITE_ROOK) : Textures.Get(PieceTextures.BLACK_ROOK)
            };
            AddInternal(Sprite);
        }

        public override List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from)
        {
            Vector2I[] dirs = {
                new Vector2I( 1,  0),
                new Vector2I(-1,  0),
                new Vector2I( 0,  1),
                new Vector2I( 0, -1),
                
            };

            return GetMovesInDirections(dirs, board, from);
        }
    }

    public partial class Queen : PieceBase
    {
        public Queen(PieceColour colour)
            : base(colour)
        {
            Type = PieceType.Queen;
        }

        protected override void LoadTexture()
        {
            Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Colour == PieceColour.White ? Textures.Get(PieceTextures.WHITE_QUEEN) : Textures.Get(PieceTextures.BLACK_QUEEN)
            };
            AddInternal(Sprite);
        }

        public override List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from)
        {
            Vector2I[] dirs = {
                new Vector2I( 1,  0),
                new Vector2I(-1,  0),
                new Vector2I( 0,  1),
                new Vector2I( 0, -1),
                new Vector2I( 1,  1),
                new Vector2I( 1, -1),
                new Vector2I(-1,  1),
                new Vector2I(-1, -1)
            };

            return GetMovesInDirections(dirs, board, from);
        }
    }

    public partial class King : PieceBase
    {
        public King(PieceColour colour)
            : base(colour)
        {
            Type = PieceType.King;
        }

        protected override void LoadTexture()
        {
            Sprite = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Colour == PieceColour.White ? Textures.Get(PieceTextures.WHITE_KING) : Textures.Get(PieceTextures.BLACK_KING)
            };
            AddInternal(Sprite);
        }

        public override List<Vector2I> GenerateMoves(PieceBase[] board, Vector2I from)
        {
            Vector2I[] dirs = {
                new Vector2I( 1,  0),
                new Vector2I(-1,  0),
                new Vector2I( 0,  1),
                new Vector2I( 0, -1),
                new Vector2I( 1,  1),
                new Vector2I( 1, -1),
                new Vector2I(-1,  1),
                new Vector2I(-1, -1)
            };

            return AddMovesFromOffsets(dirs, board, from);
        }
    }
}