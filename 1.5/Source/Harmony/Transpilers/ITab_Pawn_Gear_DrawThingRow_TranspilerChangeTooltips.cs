using System.Reflection.Emit;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(ITab_Pawn_Gear), "DrawThingRow")]
    internal static class ITab_Pawn_Gear_DrawThingRow_TranspilerChangeTooltips
    {
        /// <summary>
        /// This transpiler replaces the tooltip that shows when a piece of apparel or weapon is locked with a custom one.
        /// </summary>
        static bool CustomLockedToolTip(Rect rect, Thing gear, Pawn pawn)
        {
            var ModExt = gear.def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
            if (ModExt != null && ModExt.isGrownEquipment)
            {
                if (gear is Apparel)
                {
                    TooltipHandler.TipRegion(rect, TranslatorFormattedStringExtensions.Translate("Thek_PlenamStratusSymbiotica_ArmorLocked", pawn.LabelShort));
                    return true;
                }
                else if (gear is ThingWithComps)
                {
                    TooltipHandler.TipRegion(rect, TranslatorFormattedStringExtensions.Translate("Thek_PlenamStratusSymbiotica_WeaponLocked", pawn.LabelShort));
                    return true;
                }
            }
            return false;
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
                 * According to the IL, these instructions translate to this if statement inside ITab_Pawn_Gear_DrawThingRow.DrawThingRow()
                 * 
                 * if (Mouse.IsOver(rect2)
                 * {
                 *   // Bunch of ifs i dont feel lik writing
                 * }
                 */
                new(OpCodes.Ldloc_S),
                new(OpCodes.Call, AccessTools.Method(typeof(Mouse), nameof(Mouse.IsOver))),
                new(OpCodes.Brfalse_S)
            };


            // Our instructions we want to insert
            var instructionsToInsert = new CodeInstruction[]
            {
                new(OpCodes.Ldloc_S), // Adds local variable to the stack
                new(OpCodes.Ldarg_3), // Adds the third argument of the original method to the stack
                new(OpCodes.Ldarg_0),
                new(OpCodes.Call, AccessTools.PropertyGetter(typeof(ITab_Pawn_Gear), "SelPawnForGear")),
                new(OpCodes.Call, AccessTools.Method(typeof(ITab_Pawn_Gear_DrawThingRow_TranspilerChangeTooltips), nameof(CustomLockedToolTip))),
                // Adds my method CustomLockedToolTip that returns a bool

                new(OpCodes.Brtrue_S) // Adds a jump if the former returns true
            };


            codeMatcher.MatchStartForward(instructionsToMatch);
            // CodeMatcher rests at the first position (top one) if it finds something that matches 'instructionsToMatch'

            if (codeMatcher.IsInvalid)
            {
                // If it's invalid, it warns you and returns the regular instructions of the method
                Log.Error("ITab_Pawn_Gear_DrawThingRow_TranspilerChangeTooltips couldn't patch it's intended method, find how to report it on the Moyo workshop page.");
                return codeInstructions;
            }
            else
            {
                instructionsToInsert[0].operand = codeMatcher.Instruction.operand;
                // Gives the instruction at the first position (Ldloc_S) it's operand
                // We do so by giving it the operand of the instruction codeMatcher is resting over, as that's the same instruction we want to add here

                instructionsToInsert[5].operand = codeMatcher.InstructionAt(2).operand;
                // Gives the instruction at the forth position (Brture_S) the instruction to jump to
                // (As we're using MatchStartForward, we're at Ldloc_S,
                // and we want to jump to the same location Brfalse_S is jumping to, which (Brfalse_S) is 2 instructions further than where we're at)


                codeMatcher.Advance(3); // We get on the last instruction of the if (Mouse.IsOver(rect2)
                codeMatcher.Insert(instructionsToInsert); // We add ours
                return codeMatcher.InstructionEnumeration(); // And return the modified instructions to make the transpiler take effect
            }
        }
    }
}