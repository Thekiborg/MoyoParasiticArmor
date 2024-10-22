using Verse.AI;
using Verse.AI.Group;

namespace Thek_PSS
{
    public class JobDriver_SymbioticBerserk : JobDriver_AttackMelee
    {
        /// <summary>
        /// Modified version of AttackMelee's MakeNewToils, with an added FinishAction on the very last toil that will relaunch the job
        /// until the target pawn is dead.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Toil> MakeNewToils()
        {
            AddFinishAction(delegate
            {
                if (pawn.IsPlayerControlled && pawn.Drafted && !base.job.playerInterruptedForced)
                {
                    Thing targetThingA = base.TargetThingA;
                    if (targetThingA != null && targetThingA.def.autoTargetNearbyIdenticalThings)
                    {
                        foreach (IntVec3 item in GenRadial.RadialCellsAround(base.TargetThingA.Position, 4f, useCenter: false).InRandomOrder())
                        {
                            if (item.InBounds(base.Map))
                            {
                                foreach (Thing thing2 in item.GetThingList(base.Map))
                                {
                                    if (thing2.def == base.TargetThingA.def && pawn.CanReach(thing2, PathEndMode.Touch, Danger.Deadly) && pawn.jobs.jobQueue.Count == 0)
                                    {
                                        Job job = base.job.Clone();
                                        job.targetA = thing2;
                                        pawn.jobs.jobQueue.EnqueueFirst(job);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            });
            yield return Toils_General.DoAtomic(delegate
            {
                if (job.targetA.Thing is Pawn pawn)
                {
                    bool num = pawn.Downed && base.pawn.mindState.duty != null && base.pawn.mindState.duty.attackDownedIfStarving && base.pawn.Starving();
                    bool flag = ModsConfig.AnomalyActive && pawn.TryGetComp<CompActivity>(out CompActivity comp) && comp.IsDormant;
                    if (num || flag)
                    {
                        job.killIncappedTarget = true;
                    }
                }
            });
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            var FollowAndMeleeAttack = Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, TargetIndex.B, delegate
            {
                Thing thing = job.GetTarget(TargetIndex.A).Thing;
                if (job.reactingToMeleeThreat && thing is Pawn p && !p.Awake())
                {
                    EndJobWith(JobCondition.InterruptForced);
                }
                else if (pawn.meleeVerbs.TryMeleeAttack(thing, job.verbToUse) && pawn.CurJob != null && pawn.jobs.curDriver == this)
                {
                    Lord lord = pawn.GetLord();
                    if (lord?.LordJob != null && lord.LordJob is LordJob_Ritual_Duel lordJob_Ritual_Duel)
                    {
                        lordJob_Ritual_Duel.Notify_MeleeAttack(pawn, thing);
                    }
                }
            }).FailOnDespawnedOrNull(TargetIndex.A);
            FollowAndMeleeAttack.AddFinishAction(delegate
            {
                bool flag = job.targetA.Pawn.DeadOrDowned || job.targetA.Thing.DestroyedOrNull();
                if (!flag)
                {
                    Job job = base.job.Clone();
                    job.targetA = this.TargetA;
                    pawn.jobs.jobQueue.EnqueueFirst(job);
                    return;
                }
            });

            yield return FollowAndMeleeAttack;
        }
    }
}
