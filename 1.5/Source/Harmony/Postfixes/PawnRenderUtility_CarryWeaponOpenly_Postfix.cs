namespace Thek_PSS
{
    [HarmonyPatch(typeof(PawnRenderUtility), nameof(PawnRenderUtility.CarryWeaponOpenly))]
    internal static class PawnRenderUtility_CarryWeaponOpenly_Postfix
    {
        /// <summary>
        /// Prevents the gun of the pawn from drawing if it's part of the symbiotica's stage 5 armor set and the pawn is not actively using it
        /// </summary>
        [HarmonyPostfix]
        static void HideSymbioticaWeaponUnlessUsed(ref bool __result, Pawn pawn)
        {
            if (!__result) return;

            var gunModExt = pawn.equipment?.Primary?.def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();

            if (gunModExt != null && gunModExt.isGrownEquipment
                && !pawn.IsAttacking())
            {
                __result = false;
            }
        }
    }
}
