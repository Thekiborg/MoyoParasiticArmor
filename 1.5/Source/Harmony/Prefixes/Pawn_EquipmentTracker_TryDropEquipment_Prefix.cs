namespace Thek_PSS
{
    [HarmonyPatch(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.TryDropEquipment))]
    internal static class Pawn_EquipmentTracker_TryDropEquipment_Prefix
    {
        /// <summary>
        /// This prefix prevents the apparel from unlocking if the apparel has isStage5Set set to true
        /// </summary>
        [HarmonyPrefix]
        static bool PreventWeaponFromBeingDropped(ThingWithComps eq)
        {
            var modExt = eq.def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
            if (modExt != null && modExt.isGrownEquipment)
            {
                return false;
            }
            return true;
        }
    }
}