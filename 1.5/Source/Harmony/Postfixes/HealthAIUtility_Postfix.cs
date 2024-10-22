namespace Thek_PSS
{
    [HarmonyPatch]
    internal static class HealthAIUtility_Postfix
    {
        static IEnumerable<MethodBase> TargetMethods { [HarmonyTargetMethods] get; } = new[]
        {
            AccessTools.Method(typeof(HealthAIUtility), nameof(HealthAIUtility.WantsToBeRescued)),
            AccessTools.Method(typeof(HealthAIUtility), nameof(HealthAIUtility.ShouldBeTendedNowByPlayer))
        };

        /// <summary>
        /// This postfix makes the pawn not be tended if they have the symbiotica progression on stage 5.
        /// It specifically does:
        /// 
        /// Makes the "Colonist needs rescue" alert not show up, and prevents pawns from rescuing the downed host.
        /// Disables the "need doctor" alert, disables the tend order when right clicking with a drafted pawn 
        /// and makes WorkGiver_Tend.HasJobOnThing return false when checking for the host.
        /// </summary>
        [HarmonyPostfix]
        static void PreventSymbioticaHostsFromBeingEligibleToTend(ref bool __result, Pawn pawn)
        {
            if (pawn.health?.hediffSet?.GetFirstHediffOfDef(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica) is HediffClass_SymbioticaProgression progression
                    && progression.IsWild && progression.CurStageIndex >= progression.def.stages.Count - 1)
            {
                __result = false;
            }
        }
    }
}
