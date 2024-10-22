namespace Thek_PSS
{
#pragma warning disable CS0649

    public class IngestionOutcomeDoer_GiveHediffSymbiotica : IngestionOutcomeDoer_GiveHediff
    {
        private bool divideByBodySize;
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            HediffClass_SymbioticaProgression hediff = HediffMaker.MakeHediff(hediffDef, pawn) as HediffClass_SymbioticaProgression;
            float effect = ((!(severity > 0f)) ? hediffDef.initialSeverity : severity);
            AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize_NewTemp(pawn, toleranceChemical, ref effect, multiplyByGeneToleranceFactors, divideByBodySize);
            hediff.Severity = effect;
            hediff.IsWild = true;
            pawn.health.AddHediff(hediff);
        }
    }
}
