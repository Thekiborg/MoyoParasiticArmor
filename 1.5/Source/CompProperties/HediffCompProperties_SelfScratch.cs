namespace Thek_PSS
{
#pragma warning disable CA1051
    public class HediffCompProperties_SelfScratch : HediffCompProperties_RunOnStage
    {
        public HediffCompProperties_SelfScratch()
        {
            compClass = typeof(HediffComp_SelfScratch);
        }

        public IntRange ticksBetweenScratches; // Period it needs to wait until a new scratching surge happens
        public int amountOfInjuries; // How many injuries it's going to do per scratching surge
        public int ticksBetweenInjury = 60; // Interval to wait between the last injury and the following one
    }
#pragma warning restore CA1051
}
