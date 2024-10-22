using System.Linq;
using System.Reflection.Emit;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    internal static class FloatMenuMakerMap_AddHumanlikeOrders_EquipPostfix
    {
        static bool? hasStage5Equipment;

        private static readonly Type[] nestedTypes = typeof(FloatMenuMakerMap).GetNestedTypes(BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo targetOptionMethod
            = PlenamStratusSymbiotica.GetInternalMethods(PlenamStratusSymbiotica.AddHumanlikeOrdersMethodBase, OpCodes.Ldftn)
            .Where(IsEquipJobMethod)
            .FirstOrDefault();

        /// <summary>
        /// Removes the drop equipment order from the right click menu for pawns with the symbiotica at stage 5
        /// </summary>
        [HarmonyPostfix]
        internal static void PreventEquipOrdersToShowOnHosts(List<FloatMenuOption> opts, Pawn pawn)
        {
            foreach (FloatMenuOption opt in opts)
            {
                if (opt.action is null || targetOptionMethod == null || targetOptionMethod != opt.action.Method)
                {
                    continue;
                }


                if ((bool)HasStage5Equipment(pawn))
                {
                    opt.Label += ": " + TranslatorFormattedStringExtensions.Translate("PlenamStratusSymbiotica_CannotEquip", pawn.Named("PAWN"));
                    opt.action = null;
                }
            }
        }

        private static bool? HasStage5Equipment(Pawn pawn)
        {
            hasStage5Equipment ??= pawn.equipment?.Primary?.def.GetModExtension<PlenamStratusSymbiotica_ModExtension>()?.isGrownEquipment;

            return hasStage5Equipment is not null;
        }

        private static bool IsEquipJobMethod(MethodInfo method)
        {
            // The target method should be in a nested type, a type thats defined inside AddHumanlikeOrders
            Type declaringType = method.DeclaringType;
            if (!nestedTypes.Contains(declaringType))
                return false;
            // The target method should have a sibling method (another method in the same parent class) that has the name "Equip" with prefixes and suffixes
            // In reality it's "g__Equip|48" in 1.5 and "g__Equip|12" in 1.2
            return declaringType
              .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
              .Any(method => method.Name.Contains("Equip"));
        }
    }
}
