namespace Thek_PSS
{
    public class HediffComp_MentalStateChance : HediffComp_RunOnStage
    {
        public new HediffCompProperties_MentalStateChance Props => (HediffCompProperties_MentalStateChance)props;

        private int? lastTick;
        private int? ticksToTriggerInt;

        internal int? TicksToTriggerMentalState
        {
            get
            {
                ticksToTriggerInt ??= Props.ticksToTrigger.RandomInRange;
                return ticksToTriggerInt;
            }
            set { ticksToTriggerInt = value; }
        }


        /// <summary>
        /// It will trigger a mental state defined in the properties if:<br></br>
        /// 1. Pawn isn't downed and the hediff is in the right stage.<br></br>
        /// 2. The ticks that have passed between the last recorded tick and the hediff's current tick is greater or equal to the
        /// ticks in range for the effect to apply<br></br>
        /// 3. The number rolled by a die is higher or equal to the chance of applying the mental state defined in XML.
        /// </summary>
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Pawn.IsHashIntervalTick(180))
            {
                if (!Pawn.Downed && CanRunInHediffStage)
                {
                    lastTick ??= Find.TickManager.TicksGame;

                    if ((Find.TickManager.TicksGame - lastTick) >= TicksToTriggerMentalState)
                    {
                        float die = UnityEngine.Random.Range(0f, 1f);
                        if (Props.chanceToApply >= die)
                        {
                            Pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalStateToApply, forceWake: true);
                        }
                        Reset();
                    }
                }
            }
        }

        private void Reset()
        {
            lastTick = null;
            TicksToTriggerMentalState = null;
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref lastTick, "PlenamStratusSymbiotica_HediffComp_MentalStateChance_lastTick");
            Scribe_Values.Look(ref ticksToTriggerInt, "PlenamStratusSymbiotica_HediffComp_MentalStateChance_ticksToTriggerInt");
        }
    }
}
