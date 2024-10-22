namespace Thek_PSS
{
    [HarmonyPatch(typeof(MedicalRecipesUtility), nameof(MedicalRecipesUtility.GetFixedPartsToApplyOn))]
    internal static class MedicalRecipesUtility_GetFixedPartsToApplyOn_Postfix
    {
        /// <summary>
        /// Prevents pawns with the symbiotica from being able to receive any kind of surgery that installs an implant
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
