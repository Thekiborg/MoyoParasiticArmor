namespace Thek_PSS
{
    public class GameComponent_PlenamStratusSymbiotica : GameComponent
    {
        internal List<string> letterBodiesSent = new();
        // These just save if the letters have been sent before or not, to avoid sending 10000 letters

        public GameComponent_PlenamStratusSymbiotica(Game game)
        {
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref letterBodiesSent, "PlenamStratusSymbiotica_LetterBodiesSent", LookMode.Value);
        }
    }
}
