global using System;
global using System.Collections.Generic;
global using System.Reflection;
global using RimWorld;
global using Verse;
global using UnityEngine;
global using HarmonyLib;
using System.Linq;
using System.Reflection.Emit;

namespace Thek_PSS
{
    [StaticConstructorOnStartup]
    public static class PlenamStratusSymbiotica
    {
        internal static readonly MethodBase AddHumanlikeOrdersMethodBase = typeof(FloatMenuMakerMap).Method("AddHumanlikeOrders");

        static PlenamStratusSymbiotica()
        {
            Harmony harmony = new("Thekiborg.GrownArmor");
            harmony.PatchAll();
        }
        

        // Get children methods which the input method uses
        internal static IEnumerable<MethodInfo> GetInternalMethods(
            MethodBase method, params OpCode[] targetOpCodes)
        {
            return PatchProcessor.ReadMethodBody(method)
                .Where(x => targetOpCodes.Length == 0 || targetOpCodes.Contains(x.Key))
                .Select(x => x.Value)
                .OfType<MethodInfo>();
        }
    }
}