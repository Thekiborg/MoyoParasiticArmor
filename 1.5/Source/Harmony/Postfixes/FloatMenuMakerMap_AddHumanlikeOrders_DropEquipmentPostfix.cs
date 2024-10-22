using System.Linq;
using System.Reflection.Emit;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders")]
    internal static class FloatMenuMakerMap_AddHumanlikeOrders_DropEquipmentPostfix
    {
        private static readonly FieldInfo f_DropEquipment = typeof(JobDefOf).GetField(nameof(JobDefOf.DropEquipment));

        internal static readonly List<MethodInfo> targetOptionMethods
            = PlenamStratusSymbiotica.GetInternalMethods(PlenamStratusSymbiotica.AddHumanlikeOrdersMethodBase, OpCodes.Ldftn)
            .Distinct()
            .Where(IsDropEquipmentJobMethod)
            .ToList();

        /// <summary>
        /// Removes the drop equipment order from the right click menu for pawns with the symbiotica at stage 5
        /// </summary>
        [HarmonyPostfix]
        internal static void PreventDropEquipmentOrdersToShowOnHosts(List<FloatMenuOption> opts)
        {
            foreach (FloatMenuOption opt in opts)
            {
                if (opt.action is null || !targetOptionMethods.Contains(opt.action.Method))
                {
                    continue;
                }


                Thing thing = opt.revalidateClickTarget;
                //ThingDef thingDef = thing.def;
                if (thing is Pawn pawnClicked
                    && pawnClicked.health.hediffSet.GetFirstHediffOfDef(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica) is HediffClass_SymbioticaProgression progression
                    && progression.IsWild && progression.CurStageIndex >= progression.def.stages.Count - 1)
                {
                    opt.Label += ": " + TranslatorFormattedStringExtensions.Translate("PlenamStratusSymbiotica_CannotDropEquipment", pawnClicked.Named("PAWN"));
                    opt.action = null;
                }
            }
        }

        private static bool IsDropEquipmentJobMethod(MethodInfo method)
        {
            return PatchProcessor
              .ReadMethodBody(method)
              .Any(x => f_DropEquipment.Equals(x.Value));
        }
    }
}
