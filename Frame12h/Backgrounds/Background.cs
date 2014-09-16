using OpenTK.Graphics;

namespace Frame12h.Backgrounds
{
    public abstract class Background
    {
        protected Game game;

        public virtual int Layers { get; set; }

        public virtual Color4 Color { get; set; }

        public virtual double Speed { get; set; }

        public virtual int FadeTime { get; set; }

        public Background(Game game)
        {
            this.game = game;
            Color = Color4.White;
            Speed = 1;
            FadeTime = 300;
            Layers = 1;
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract void Update();

        public abstract void Render();
    }
}
