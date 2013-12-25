namespace Frame12h.Interface
{
    public abstract class Menu
    {
        protected class TextInput
        {
            private string input;
            private int cursorPos;
            private int scrollPos;
            private int selectionStart;
            private int selectionLength;

            private int x;
            private int y;
            
        }

        protected Game game;

        public Menu(Game game)
        {
            this.game = game;
        }

        public abstract void Update();

        public abstract void Render();
    }
}
