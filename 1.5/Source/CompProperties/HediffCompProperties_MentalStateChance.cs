namespace Thek_PSS
{
#pragma warning disable CA1051
    public class HediffCompProperties_MentalStateChance : HediffCompProperties_RunOnStage
    {
        public HediffCompProperties_MentalStateChance()
        {
            compClass = typeof(HediffComp_MentalStateChance);
        }

        public IntRange ticksToTrigger; // Every how many ticks it will try to apply the mental state
        public float chanceToApply; // The chance for the mental state to apply
        public MentalStateDef mentalStateToApply;
    }
#pragma warning restore CA1051
}
