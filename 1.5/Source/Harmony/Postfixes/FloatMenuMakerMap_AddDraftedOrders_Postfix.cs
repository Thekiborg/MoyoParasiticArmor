using System.Linq;
using System.Reflection.Emit;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "AddDraftedOrders")]
    internal static class FloatMenuMakerMap_AddDraftedOrders_Postfix
    {
        private static readonly MethodBase originalMethod = typeof(FloatMenuMakerMap).Method("AddDraftedOrders");
        private static readonly FieldInfo f_TendPatient = typeof(JobDefOf).GetField(nameof(JobDefOf.TendPatient));

        internal static readonly List<MethodInfo> targetOptionMethods
            = PlenamStratusSymbiotica.GetInternalMethods(originalMethod, OpCodes.Ldftn)
            .Distinct()
            .Where(IsTendPatientJobMethod)
            .ToList();


        /// <summary>
        /// Removes the tend drafted order from the right click menu when clicking on a pawn with the symbiotica at stage 5
        /// </summary>
        [HarmonyPostfix]
        internal static void PreventTendDraftedOrdersToShowOnHosts(List<FloatMenuOption> opts)
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
                    opt.Label += ": " + TranslatorFormattedStringExtensions.Translate("PlenamStratusSymbiotica_CannotTend", pawnClicked.Named("TARGETPAWN"));
                    opt.action = null;
                }
            }
        }

        private static bool IsTendPatientJobMethod(MethodInfo method)
        {
            return PatchProcessor
              .ReadMethodBody(method)
              .Any(x => f_TendPatient.Equals(x.Value));
        }
    }
}
