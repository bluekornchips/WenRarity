namespace WenRarityLibrary.Builders
{
    public class FrameworkDirector
    {
        private static FrameworkDirector instance;
        public static FrameworkDirector Instance => instance ?? (instance = new FrameworkDirector());
        private FrameworkDirector() { }

        private static Ducky _ducky = Ducky.Instance;
        public BlockfrostFrameworkBuilder bf = BlockfrostFrameworkBuilder.Instance;
        public RimeFrameworkBuilder rime = RimeFrameworkBuilder.Instance;
    }
}