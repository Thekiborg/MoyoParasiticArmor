namespace Thek_PSS
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    public abstract class HediffCompProperties_RunOnStage : HediffCompProperties
    {
        public List<int> stageIndicesToRun = new();
        public bool happensOnWild;
        public bool happensOnControlled;
    }
#pragma warning restore CA1051 // Do not declare visible instance fields
}
