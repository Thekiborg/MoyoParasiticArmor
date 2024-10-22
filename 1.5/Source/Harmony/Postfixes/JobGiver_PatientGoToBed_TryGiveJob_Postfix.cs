using Verse.AI;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(JobGiver_PatientGoToBed), "TryGiveJob")]
    internal static class JobGiver_PatientGoToBed_TryGiveJob_Postfix
    {
        /// <summary>
        /// Prevents pawns from bed resting if the hediff is in the armor stage (5th stage). Pawn will rest if they have a programmed surgery.
        /// </summary>
        [HarmonyPostfix]
        internal static void PreventSymbioticaHostFromBedResting(ref Job __result, Pawn pawn)
        {
            if (pawn.health?.hediffSet?.GetFirstHediffOfDef(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica) is HediffClass_SymbioticaProgression progression
                    && progression.IsWild && progression.CurStageIndex >= progression.def.stages.Count - 1)
            {
                __result = null;
            }
        }
    }
}
