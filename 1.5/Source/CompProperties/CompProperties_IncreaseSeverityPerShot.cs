namespace Thek_PSS
{
#pragma warning disable CA1051
    public class CompProperties_IncreaseSeverityPerShot : CompProperties
    {
        public CompProperties_IncreaseSeverityPerShot()
        {
            compClass = typeof(Comp_IncreaseSeverityPerShot);
        }

        public HediffDef hediffDefToIncrease;
        public float severityIncreasePerShot;
    }
#pragma warning restore CA1051
}
