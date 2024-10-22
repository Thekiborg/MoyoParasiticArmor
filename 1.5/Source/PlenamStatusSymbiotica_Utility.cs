using System.Linq;

namespace Thek_PSS
{
    internal static class PlenamStatusSymbiotica_Utility
    {
        /// <summary>
        /// Used to find the spawned pawn with the highest medicine skill between all the maps
        /// </summary>
        /// <param name="infectedPawn">The pawn who has the Symbiotica Progression can't be the same one as the DOCTOR in the letters.</param>
        /// <returns>The best medicine skilled pawn who's not the same pawn as the one passed as an argument</returns>
        internal static Pawn GetHighestMedicalNotInfectedPawn(Pawn infectedPawn)
        {
            return PawnsFinder.AllMaps_FreeColonistsSpawned
                .Where(p => p != infectedPawn && !p.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled)
                .OrderBy(p => p.skills.GetSkill(SkillDefOf.Medicine))
                .LastOrDefault() ?? PawnsFinder.AllMaps_FreeColonistsSpawned.Last();
        }
    }
}
