namespace Thek_PSS
{
    [HarmonyPatch(typeof(Recipe_RemoveBodyPart), nameof(Recipe_RemoveBodyPart.GetPartsToApplyOn))]
    internal static class Recipe_RemoveBodyPart_GetPartsToApplyOn_Postfix
    {
        /// <summary>
        /// Prevents pawns with the symbiotica from being able to receive any kind of surgery that removes a body part
        /// </summary>
        [HarmonyPostfix]
        internal static void ClearPartsToApplyOnSymbioticaHosts(ref IEnumerable<BodyPartRecord> __result, Pawn pawn)
        {
            if (pawn.health?.hediffSet?.GetFirstHediffOfDef(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica) is HediffClass_SymbioticaProgression progression
                    && progression.IsWild && progression.CurStageIndex >= progression.def.stages.Count - 1)
            {
                IEnumerable<BodyPartRecord> emptyIenum = [];
                __result = emptyIenum;
            }
        }
    }
}
