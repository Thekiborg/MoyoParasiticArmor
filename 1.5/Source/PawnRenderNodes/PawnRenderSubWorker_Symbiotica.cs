namespace Thek_PSS
{
    public class PawnRenderSubWorker_Symbiotica : PawnRenderSubWorker
    {
        public override bool CanDrawNowSub(PawnRenderNode node, PawnDrawParms parms)
        {
            if (base.CanDrawNowSub(node, parms))
            {
                Hediff hediff = null;

                if (parms.pawn.Downed)
                {
                    return true;
                }
                if (parms.pawn.Drafted || parms.pawn.IsAttacking())
                {
                    return true;
                }
                parms.pawn.health?.hediffSet?.TryGetHediff(PlenamStratusSymbioticaHediffDefOfs.Thek_PlenamStratusSymbiotica, out hediff);
                if (hediff != null && parms.pawn.MentalStateDef == hediff.TryGetComp<HediffComp_MentalStateChance>().Props.mentalStateToApply)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
