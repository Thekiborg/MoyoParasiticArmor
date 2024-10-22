using Verse.AI;

namespace Thek_PSS
{
    public class ThinkNode_ChancePerCustomTicks : ThinkNode_Priority
    {
        private const float ticksPerMTB = 2500f;

        /// <summary>
        /// Same code as ThinkNode_ChancePerHour, but having the hardcoded 2500 ticks replaced with a modExtension field, to be configurable from the XML.
        /// </summary>
        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            var modExt = PlenamStratusSymbioticaThinkTreeDefOfs.MentalStateCritical.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
            if (Find.TickManager.TicksGame < GetLastTryTick(pawn) + modExt.runInterval)
            {
                return ThinkResult.NoJob;
            }
            SetLastTryTick(pawn, Find.TickManager.TicksGame);
            if (modExt.mtbHours <= 0f)
            {
                return ThinkResult.NoJob;
            }
            Rand.PushState();
            int salt = Gen.HashCombineInt(base.UniqueSaveKey, 694206942);
            Rand.Seed = pawn.RandSeedForHour(salt);
            bool num2 = Rand.MTBEventOccurs(modExt.mtbHours, ticksPerMTB, modExt.runInterval);
            Rand.PopState();
            if (num2)
            {
                return base.TryIssueJobPackage(pawn, jobParams);
            }
            return ThinkResult.NoJob;
        }
        private int GetLastTryTick(Pawn pawn)
        {
            if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out var value))
            {
                return value;
            }
            return -99999;
        }

        private void SetLastTryTick(Pawn pawn, int val)
        {
            pawn.mindState.thinkData[base.UniqueSaveKey] = val;
        }
    }
}
