namespace Thek_PSS
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    public class HediffCompProperties_SymbioticRegeneration : HediffCompProperties_RunOnStage
    {
        public HediffCompProperties_SymbioticRegeneration()
        {
            compClass = typeof(HediffComp_SymbioticRegeneration);
        }

        public IntRange ticksToPassToRegen;
        public int ticksToCheck = 2500; // An hour
    }
#pragma warning restore CA1051 // Do not declare visible instance fields
}
