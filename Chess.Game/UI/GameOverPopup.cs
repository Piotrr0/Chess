using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace Chess.Game.UI.GameOver
{
    public partial class GameOverPopup : CompositeDrawable
    {
        private SpriteText messageText;
        private string gameOverText = "Game Over";

        public double InterpolationDuration = 400d;
        public Vector2 PopupSize = new Vector2(720, 400);
        public Colour4 PopupColour = new Colour4(0f, 0f, 0f, 0.8f);
        public Vector2 BaseScale = new Vector2(0.2f);
        public float GameOverTextFontSize = 80f;
        public float MessageTextFontSize = 50f;

        public GameOverPopup(string message)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Alpha = 0;
            Size = PopupSize;
            Scale = BaseScale;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = PopupColour
                },
                new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = gameOverText,
                            Font = FontUsage.Default.With(size: GameOverTextFontSize),
                            Colour = Colour4.White,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },
                        (messageText = new SpriteText
                        {
                            Text = message,
                            Font = FontUsage.Default.With(size: MessageTextFontSize),
                            Colour = Colour4.LightGray,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        })
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            this.FadeIn(InterpolationDuration, Easing.OutQuint);
            this.ScaleTo(1f, InterpolationDuration, Easing.OutBack);
        }

        public void SetMessage(string message)
        {
            messageText.Text = message;
        }
    }
}