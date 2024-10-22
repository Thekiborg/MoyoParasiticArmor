using Verse.AI;

namespace Thek_PSS
{
    public class JobGiver_SymbioticBerserk : JobGiver_Berserk
    {
        /// <summary>
        /// Removes the 0.5 chance of the job returning Wait_Combat instead of AttackMelee.<br></br>
        /// Also prevents the jobgiver from giving out a job if a limit of max attacked pawns has been reached.
        /// </summary>
        protected override Job TryGiveJob(Pawn pawn)
        {
            JobDef jobDef = PlenamStratusSymbioticaJobDefOfs.Thek_PlenamStratusSymbiotica_SymbioticBerserk;
            MentalState_SymbioticBerserk mentalState = pawn.MentalState as MentalState_SymbioticBerserk;

           mentalState.totalAttacks ??= jobDef.GetModExtension<PlenamStratusSymbiotica_ModExtension>().numberOfAttacks.RandomInRange;

            if (mentalState?.numberOfAttacksPerformed >= mentalState?.totalAttacks)
            {
                return null;
            }
            if (pawn.TryGetAttackVerb(null) is null)
            {
                return null;
            }
            Thing thing = FindAttackTarget(pawn);
            if (thing is not null)
            {
                mentalState.numberOfAttacksPerformed += 1;
                Job job2 = JobMaker.MakeJob(jobDef, thing);
                job2.canBashDoors = true;
                return job2;
            }
            return null;
        }

        /// <summary>
        /// Reduces the area the pawn has to look for a target.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        private Thing FindAttackTarget(Pawn pawn)
        {
            return (Thing)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedReachable, IsGoodTarget, 0f, 15f, default(IntVec3), float.MaxValue, canBashDoors: true);
        }

        /// <summary>
        /// JobGiver_GetFood's max priority is 9.5, so this will always run after JobGiver_GetFood if the pawn is hungry.
        /// </summary>
        public override float GetPriority(Pawn pawn)
        {
            return 9f;
        }
    }
}
