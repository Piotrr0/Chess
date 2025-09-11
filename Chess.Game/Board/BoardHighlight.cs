
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace Chess.Game.Board.Highlight
{

    public interface IHighlightManager
    {
        void HighlightCells(List<Vector2> positions);
        void UnHighlightCells(List<Vector2> positions);
        void ClearAll();
    }


    public class HighlightManager : IHighlightManager
    {
        private readonly List<HighlightMarker> highlights = new List<HighlightMarker>();
        private readonly Container boardContainer;

        public HighlightManager(Container boardContainer)
        {
            this.boardContainer = boardContainer;
        }

        public void HighlightCells(List<Vector2> positions)
        {
            foreach (Vector2 pos in positions)
            {
                HighlightMarker highlight = new HighlightMarker(pos);
                highlights.Add(highlight);
                boardContainer.Add(highlight);
            }
        }

        public void UnHighlightCells(List<Vector2> positions)
        {
            List<HighlightMarker> toRemove = highlights.Where(h => positions.Contains(h.Position)).ToList();

            foreach (HighlightMarker highlight in toRemove)
            {
                highlight.Expire();
                highlights.Remove(highlight);
            }
        }

        public void ClearAll()
        {
            foreach (var h in highlights)
                h.Expire();

            highlights.Clear();
        }
    }


    public partial class HighlightMarker : CompositeDrawable
    {
        private float size = ChessBoardGlobals.SQUARE_SIZE;

        public HighlightMarker(Vector2 position)
        {
            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;
            Position = position + new Vector2(size/4, size/4);

            InternalChild = new Circle
            {
                Colour = Colour4.Gray,
                Size = new Vector2(size/2, size/2),
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,                
            };
        }
    }
}