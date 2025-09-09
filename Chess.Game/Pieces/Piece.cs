using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Graphics.Textures;
using Chess.Game.Pieces.Textures;
using osu.Framework.Allocation;

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
        public PieceType Type { get; protected set; } = PieceType.None;
        public PieceColour Colour { get; set; } = PieceColour.None;
        public Sprite Sprite { get; protected set; }

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
    }
}