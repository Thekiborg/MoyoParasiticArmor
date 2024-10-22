namespace Thek_PSS
{
    [HarmonyPatch(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.Unlock))]
    internal static class Pawn_ApparelTracker_UnlockAll_Prefix
    {
        /// <summary>
        /// This prefix prevents the apparel from unlocking if the apparel has isStage5Set set to true
        /// </summary>
        [HarmonyPrefix]
        static bool PreventArmorFromBeingDropped(Apparel apparel)
        {
            var apparelModExt = apparel.def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
            if (apparelModExt != null && apparelModExt.isGrownEquipment)
            {
                return false;
            }
            return true;
        }
    }
}