using AlienRace.ExtendedGraphics;
using Verse;

namespace Thek_PSS
{
    public class ConditionSymbiotica : Condition
    {
        public new const string XmlNameParseKey = "DontShowWithPSSstage5";

        public override bool Satisfied(ExtendedGraphicsPawnWrapper pawn, ref ResolveData data)
        {
            if (pawn.WrappedPawn.health.hediffSet.GetFirstHediffOfDef(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica) is HediffClass_SymbioticaProgression hediff
                && hediff.IsWild && hediff.CurStageIndex >= hediff.def.stages.Count - 1)
            {
                if (pawn.WrappedPawn.Downed)
                {
                    return false;
                }
                if (pawn.WrappedPawn.Drafted || pawn.WrappedPawn.IsAttacking())
                {
                    return false;
                }
                if (pawn.WrappedPawn.MentalStateDef == hediff.TryGetComp<HediffComp_MentalStateChance>().Props.mentalStateToApply)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
