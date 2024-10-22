namespace Thek_PSS
{
    public class ThoughWorker_SymbioticaSick : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.health.hediffSet.GetFirstHediffOfDef(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica) is not HediffClass_SymbioticaProgression symbioticaHediff
                || !symbioticaHediff.IsWild)
            {
                return ThoughtState.Inactive;
            }
            else
            {
                return ThoughtState.ActiveAtStage(symbioticaHediff.CurStageIndex - 1);
                // My hediff has 5 stages, while the thought has 4 stages, i need to substract 1 to compensate for this

                /* Dummy Stage -> index 0
                 * Incubation Stage -> index 1
                 * First Stage -> index 2
                 * Second Stage -> index 3
                 * Third (Final) Stage -> index 4
                 */

                // If the hediff is on the second stage (index 3), it would look for the third stage's thought otherwise
            }
        }
    }
}
