using System.Reflection.Emit;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(Pawn_ApparelTracker), nameof(Pawn_ApparelTracker.DropAll))]
    internal static class Pawn_ApparelTracker_DropAll_Transpiler
    {
        /// <summary>
        /// Prevent the armor specified in the mod extension from being droppable
        /// 
        /// This patch is getting the list tmpApparelList after all the apparel has been added into it by the game,
        /// to remove the apparel with the tag isStage5Gear, which should not be droppable.
        /// </summary>
        static void InsertLoopToRemoveGrownApparel(List<Apparel> tmpApparelList)
        {
            for (int i = tmpApparelList.Count - 1; i >= 0; i--)
            {
                PlenamStratusSymbiotica_ModExtension apparelModExt = tmpApparelList[i].def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
                if (apparelModExt != null && apparelModExt.isGrownEquipment)
                {
                    tmpApparelList.RemoveAt(i);
                }
            }
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> InsertLoop_Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            // Rather than finding the instructions directly
            CodeMatcher codeMatcher = new(codeInstructions);

            // The instructions it needs to find to know where to do the stuff
            var instructionsToMatch = new CodeMatch[]
            {
                /*
                 * According to the IL, these instructions translate to this for loop inside Pawn_ApparelTracker.DropAll()
                 * 
                 * for (int i = 0; i < wornApparel.Count; i++) <-- The 5 instructions are specifically this 'i < wornApparel.Count'
                 * {
                 *  if ((dropLocked || !IsLocked(wornApparel[i])) && (selector == null || selector(wornApparel[i])))
                 *  {
                 *      tmpApparelList.Add(wornApparel[i]);
                 *  }
                 * }
                 */
                new(OpCodes.Ldloc_0),
                new(OpCodes.Ldarg_0),
                new(OpCodes.Ldfld, AccessTools.Field(typeof(Pawn_ApparelTracker), "wornApparel")),
                new(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(ThingOwner), nameof(ThingOwner.Count))),
                new(OpCodes.Blt_S)
            };

            // Our instructions we want to insert
            var instructionsToInsert = new CodeInstruction[]
            {
                new(OpCodes.Ldsfld, AccessTools.Field(typeof(Pawn_ApparelTracker), "tmpApparelList")), // Argument for the method
                new(OpCodes.Call, AccessTools.Method(typeof(Pawn_ApparelTracker_DropAll_Transpiler), nameof(InsertLoopToRemoveGrownApparel))) // Method we want to insert
            };

            codeMatcher.MatchEndForward(instructionsToMatch);

            if (codeMatcher.IsInvalid)
            {
                // If it's invalid, it warns you and returns the regular instructions of the method
                Log.Error("PawnApparelTracker_DropAll_Transpiler couldn't patch it's intended method, please contact Thekiborg on discord.");
                return codeInstructions;
            }
            else
            {
                codeMatcher.Advance(1);
                // MatchEndForwards stops just when it finds those instructions, this makes it travel 1 instruction forwards after our target, immediately after what we've found
                codeMatcher.Insert(instructionsToInsert);
                // Inserts our own instructions
                return codeMatcher.InstructionEnumeration();
            }
        }
    }
}