using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frame12h.Backgrounds
{
    public class StarsBackground : Background
    {
        class StarLayer
        {
            class Star
            {
                public double X;

                public double Y;

                public Color4 Color;

                public Star(double x, double y, Color4 color)
                {
                    X = x;
                    Y = y;
                    Color = color;
                }
            }

            private Game game;

            private Star[] stars;

            private double speed;

            private Color4 color;

            private Random rnd;

            private int timeToDelete = -1;

            private bool opacityChange = false;

            private float opacity = 0.0f;

            private float opacityStep = 0.0f;

            public bool Deleted
            {
                get { return timeToDelete == 0; }
            }

            public StarLayer(Game game, Random rnd, int starsNumber, Color4 color, double speed)
            {
                this.game = game;
                this.rnd = rnd;
                this.speed = speed;
                this.color = color;
                stars = new Star[starsNumber];
                for (var i = 0; i < stars.Length; i++)
                {
                    var col = color;
                    col.R = (float)(color.R * (1.0 - 0.2 * rnd.NextDouble()));
                    col.G = (float)(color.G * (1.0 - 0.2 * rnd.NextDouble()));
                    col.B = (float)(color.B * (1.0 - 0.2 * rnd.NextDouble()));
                    stars[i] = new Star(rnd.Next(Game.ProjectionWidth), rnd.Next(Game.ProjectionHeight), col);
                }
            }

            public void SetColor(Color4 newColor)
            {

            }

            public void Start(int time)
            {
                opacityChange = true;
                opacityStep = 1.0f / time;
            }

            public void Stop(int time)
            {
                opacityChange = true;
                opacityStep = -1.0f / time;
            }

            public void Delete(int time)
            {
                Stop(time);
                timeToDelete = time;
            }

            public void Update()
            {
                if (opacityChange)
                {
                    opacity += opacityStep;
                    if (opacity >= 1.0f)
                    {
                        opacity = 1.0f;
                        opacityChange = false;
                    }
                    else if (opacity <= 0.0f)
                    {
                        opacity = 0.0f;
                        opacityChange = false;
                    }
                    else
                        foreach(var star in stars)
                        {
                            star.Color.R = color.R * opacity;
                            star.Color.G = color.G * opacity;
                            star.Color.B = color.B * opacity;
                        }
                }

                foreach (var star in stars)
                {
                    star.Y += speed;
                    if (star.Y > Game.ProjectionHeight)
                    {
                        star.X = rnd.Next(Game.ProjectionWidth);
                        star.Y = 0;
                    }
                }

                if (timeToDelete > 0)
                    timeToDelete--;
            }

            public void Render()
            {
                GL.Begin(BeginMode.Points);

                foreach (var star in stars)
                {
                    if (opacityChange)
                        GL.Color4(star.Color.R * opacity, star.Color.G * opacity, star.Color.B * opacity, 0.0f);
                    else
                        GL.Color4(star.Color);
                    GL.Vertex2(star.X, star.Y);
                }

                GL.End();
            }
        }

        private bool active = false;

        private Random rnd = new Random();

        private List<StarLayer> starLayers = new List<StarLayer>();

        public override int Layers
        {
            get
            {
                return base.Layers;
            }
            set
            {
                base.Layers = value;
                if (starLayers.Count < value)
                    for (var i = starLayers.Count; i < value; i++)
                    {
                        var layer = new StarLayer(game, rnd, GetStarsNumberForLayer(i), GetStarsColorForLayer(i), GetStarsSpeedForLayer(i));
                        starLayers.Add(layer);
                        if (active)
                            layer.Start(FadeTime);
                    }
                else if (starLayers.Count > value)
                    for (var i = starLayers.Count; i >= value; i--)
                        starLayers[i].Delete(FadeTime);
            }
        }

        public StarsBackground(Game game)
            : base(game)
        {
            Layers = 5;
        }

        public override void Start()
        {
            active = true;
            foreach (var layer in starLayers)
                layer.Start(FadeTime);
        }

        public override void Stop()
        {
            active = false;
            foreach (var layer in starLayers)
                layer.Stop(FadeTime);
        }

        public override void Update()
        {
            foreach (var layer in starLayers)
                if (layer.Deleted)
                    starLayers.Remove(layer);
                else
                    layer.Update();
        }

        public override void Render()
        {
            foreach (var layer in starLayers.Reverse<StarLayer>())
                layer.Render();
        }

        private int GetStarsNumberForLayer(int layer)
        {
            return 25;
        }

        private Color4 GetStarsColorForLayer(int layer)
        {
            var color = Color;
            for (var i = 0; i < layer; i++)
            {
                color.R *= 0.75f;
                color.G *= 0.75f;
                color.B *= 0.75f;
            }
            return color;
        }

        private double GetStarsSpeedForLayer(int layer)
        {
            var speed = Speed;
            for (var i = 0; i < layer; i++)
            {
                speed *= 0.75;
            }
            return speed;
        }
    }
}
