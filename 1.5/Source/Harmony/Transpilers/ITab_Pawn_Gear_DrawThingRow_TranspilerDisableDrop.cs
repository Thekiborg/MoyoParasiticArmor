using System.Linq;
using System.Reflection.Emit;

namespace Thek_PSS
{
    [HarmonyPatch(typeof(ITab_Pawn_Gear), "DrawThingRow")]
    internal static class ITab_Pawn_Gear_DrawThingRow_TranspilerDisableDrop
    {
        /// <summary>
        /// This transpiler adds a check in the method's 'flag4', to check if the method is part of the symbiotica set.<br></br>
        /// If it is, the flag will be set to true which will disale the button to drop the weapon.
        /// </summary>
        static bool ExtraConditionOnFlag(Thing thing)
        {
            var modExt = thing.def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
            if (thing is ThingWithComps && modExt != null && modExt.isGrownEquipment)
            {
                return true;
            }
            return false;
        }


        private static int StaticAdj(MethodBase method)
        {
            return method.IsStatic ? 0 : 1;
        }


        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> InsertLoop_Transpiler(IEnumerable<CodeInstruction> codeInstructions, MethodBase methodBase)
        {
            // Rather than finding the instructions directly
            CodeMatcher codeMatcher = new(codeInstructions);

            // The instructions it needs to find
            var instructionsToMatch = new CodeMatch[]
            {
                new(OpCodes.Ldc_I4_0),

                new(OpCodes.Stloc_S),
                new(OpCodes.Ldloc_S),
                new(OpCodes.Ldloc_S),
                new(OpCodes.Or),
                new(OpCodes.Ldloc_S),
                new(OpCodes.Or),
                new(OpCodes.Stloc_2),
            };
            

            codeMatcher.MatchEndForward(instructionsToMatch); // Goes to the instruction where we want to insert
            var jumpOperandTarget = codeMatcher.Instruction.operand;

            var thingArgument = methodBase.GetParameters().First(static param => param.ParameterType == typeof(Thing)).Position + StaticAdj(methodBase);

            // Our instructions we want to insert
            var instructionsToInsert = new CodeInstruction[]
            {
                CodeInstruction.LoadArgument(thingArgument),
                new(OpCodes.Call, AccessTools.Method(typeof(ITab_Pawn_Gear_DrawThingRow_TranspilerDisableDrop), nameof(ExtraConditionOnFlag))),
                new(OpCodes.Or)
            };


            if (codeMatcher.IsInvalid)
            {
                // If it's invalid, it warns you and returns the regular instructions of the method
                Log.Error("ITab_Pawn_Gear_DrawThingRow_TranspilerDisableDrop couldn't patch it's intended method, find how to report it on the Moyo workshop page.");
                return codeInstructions;
            }
            else
            {
                // We don't advance, so we insert the instructions right where we're resting, pushing the instruction on our position down and putting ours instead
                codeMatcher.Insert(instructionsToInsert);

                return codeMatcher.InstructionEnumeration();
            }
        }
    }
}
