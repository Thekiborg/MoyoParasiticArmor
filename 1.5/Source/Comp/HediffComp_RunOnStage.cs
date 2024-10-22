namespace Thek_PSS
{
    public abstract class HediffComp_RunOnStage : HediffComp
    {
        public HediffCompProperties_RunOnStage Props => (HediffCompProperties_RunOnStage)props;
        private int cachedHediffStage = -1;
        private bool cachedCanRunCompInt;

        /// <summary>
        /// Caching so I don't run a loop every so often, but when the hediff stage changes
        /// </summary>
        internal bool CanRunInHediffStage
        {
            get
            {
                // If the hediff is the symbiotica, it will check if it's wild or not
                if (parent is HediffClass_SymbioticaProgression hediff)
                {
                    if (hediff.IsWild && !Props.happensOnWild || !hediff.IsWild && !Props.happensOnControlled)
                    {
                        return false;
                    }

                    if (parent.CurStageIndex != cachedHediffStage)
                    {
                        cachedHediffStage = parent.CurStageIndex;

                        cachedCanRunCompInt = Props.stageIndicesToRun.Contains(parent.CurStageIndex);
                        // Returns true if the stage index the hediff is on is one of the indices in the list
                    }
                }
                else // HediffComps should be able to be used on more hediffs than the parasite doing this
                {
                    if (parent.CurStageIndex != cachedHediffStage)
                    {
                        cachedHediffStage = parent.CurStageIndex;

                        cachedCanRunCompInt = Props.stageIndicesToRun.Contains(parent.CurStageIndex);
                        // Returns true if the stage index the hediff is on is one of the indices in the list
                    }
                }

                return cachedCanRunCompInt;
            }
        }
    }
}