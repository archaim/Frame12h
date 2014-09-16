using Frame12h.Backgrounds;
using Frame12h.Interface;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace Frame12h
{
    public class Game : GameWindow
    {
        public const int ProjectionWidth = 800;
        public const int ProjectionHeight = 600;

        public Menu Menu { get; set; }

        public Background Background { get; set; }

        [STAThread]
        static void Main()
        {
            using (var Game = new Game())
            {
                Game.Run(60);
            }
        }

        public Game()
            : base(ProjectionWidth, ProjectionHeight, GraphicsMode.Default, "Frame12h")
        {
            ClientSize = new Size(Settings.WindowWidth, Settings.WindowHeight);
            WindowState = Settings.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;

            VSync = VSyncMode.On;

            Menu = new TitleMenu(this);

            Background = new StarsBackground(this);
            Background.Color = Color4.Yellow;
            Background.Layers = 10;
            Background.Start();
        }

        protected override void OnLoad(EventArgs E)
        {
            base.OnLoad(E);
        }

        protected override void OnResize(EventArgs E)
        {
            base.OnResize(E);

            var width = ClientRectangle.Width;
            var height = (int)Math.Round((double)ProjectionHeight / ProjectionWidth * width);
            if (height > ClientRectangle.Height)
            {
                height = ClientRectangle.Height;
                width = (int)Math.Round((double)ProjectionWidth / ProjectionHeight * height);
            }

            var dx = (ClientRectangle.Width - width) / 2;
            var dy = (ClientRectangle.Height - height) / 2;
            GL.Viewport(ClientRectangle.X + dx, ClientRectangle.Y + dy, width, height);
        }

        protected override void OnUpdateFrame(FrameEventArgs E)
        {
            base.OnUpdateFrame(E);
            if (Keyboard[Key.Escape])
                Exit();
            if ((Keyboard[Key.AltLeft] || Keyboard[Key.AltRight]) && Keyboard[Key.Enter])
                if (this.WindowState != WindowState.Fullscreen)
                    this.WindowState = WindowState.Fullscreen;
                else
                    this.WindowState = WindowState.Normal;

            Menu.Update();

            Background.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs E)
        {
            base.OnRenderFrame(E);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var Projection = Matrix4.CreateOrthographic(-ProjectionWidth, -ProjectionHeight, -1, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref Projection);
            GL.Translate(ProjectionWidth / 2 - 0.5, -(ProjectionHeight / 2 - 0.5), 0);

            var Modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref Modelview);

            DrawBorder();

            Menu.Render();

            Background.Render();

            SwapBuffers();
        }

        private void DrawBorder()
        {
            GL.Begin(BeginMode.LineLoop);

            GL.Color4(Color4.LimeGreen);

            GL.Vertex2(0, 0);
            GL.Vertex2(ProjectionWidth - 1, 0);
            GL.Vertex2(ProjectionWidth - 1, ProjectionHeight - 1);
            GL.Vertex2(0, ProjectionHeight - 1);

            GL.End();
        }
    }
}
