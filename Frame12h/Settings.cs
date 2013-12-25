using System.Configuration;

namespace Frame12h
{
    public static class Settings
    {
        public static int WindowWidthDefault = 800;
        public static int WindowHeightDefault = 600;
        public static bool FullscreenDefault = false;

        public static int WindowWidth
        {
            get
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings["WindowWidth"], out value))
                    return value;
                else
                    return WindowWidthDefault;
            }
        }

        public static int WindowHeight
        {
            get
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings["WindowHeight"], out value))
                    return value;
                else
                    return WindowHeightDefault;
            }
        }

        public static bool Fullscreen
        {
            get
            {
                bool value;
                if (bool.TryParse(ConfigurationManager.AppSettings["Fullscreen"], out value))
                    return value;
                else
                    return FullscreenDefault;
            }
        }
    }
}
