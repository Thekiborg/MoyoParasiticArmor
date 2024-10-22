namespace Thek_PSS
{
    public class HediffComp_SelfScratch : HediffComp_RunOnStage
    {
        public new HediffCompProperties_SelfScratch Props => (HediffCompProperties_SelfScratch)props;
        private int? lastTick;
        private int? scratchingSurgeIntervalInt;

        private int injuriesToGive; // Used to count down the amount of times to make the pawn scratch itself, in a separate hashinterval
        
        /// <summary>
        /// Gets a random tick for the surge to happen at.
        /// </summary>
        private int? ScratchingSurgeInterval
        {
            get
            {
                scratchingSurgeIntervalInt ??= Props.ticksBetweenScratches.RandomInRange;
                return scratchingSurgeIntervalInt;
            }
            set { scratchingSurgeIntervalInt = value; }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Pawn.IsHashIntervalTick(180))
            {
                if (!Pawn.Downed && CanRunInHediffStage)
                {
                    lastTick ??= parent.ageTicks; // Saves the last tick

                    // If the difference between the hediff's current ticks and the last recorded ticks
                    // (The amount of time that has passed from the last surge)
                    // Is greater than the tick at which it needs to happen.
                    if ((parent.ageTicks - lastTick) >= ScratchingSurgeInterval)
                    {
                        lastTick = parent.ageTicks; // Saves the last tick again
                        ScratchingSurgeInterval = null; // Makes the surge interval null to fetch a new one.

                        injuriesToGive = Props.amountOfInjuries; // Loads the amount of injuries we want to give
                    }
                }
            }

            // If there are any injuries to give and enough time has passed
            if (injuriesToGive > 0 && Pawn.IsHashIntervalTick(Props.ticksBetweenInjury) && CanRunInHediffStage)
            {
                StunAndDamagePawn();
                injuriesToGive--;
                // We substract one from the injuries to give
            }
        }

        /// <summary>
        /// The method does two things:<br></br>
        /// First, it will stun the pawn for ticksBetweenInjury (60) multiplied by amountOfInjuries to give (5) = 5 seconds stunned, as an example.<br></br>
        /// Second, it will use a custom damagedef that always hits the pawn's body, doesn't make scars, and doesn't do any damage to clothes
        /// </summary>
        private void StunAndDamagePawn()
        {
            Pawn.stances.stunner.StunFor(Props.ticksBetweenInjury * Props.amountOfInjuries, Pawn);
            Pawn.TakeDamage(new DamageInfo(PlenamStratusSymbioticaDamageDefOfs.Thek_PlenamStratusSymbiotica_SelfScratch, 0.5f, armorPenetration: int.MaxValue, instigator: Pawn, instigatorGuilty: false, spawnFilth: false));
        }


        public override void CompExposeData()
        {
            Scribe_Values.Look(ref lastTick, "PlenamStratusSymbiotica_HediffComp_SelfScratch_lastTick");
            Scribe_Values.Look(ref scratchingSurgeIntervalInt, "PlenamStratusSymbiotica_HediffComp_SelfScratch_scratchingSurgeIntervalInt");
        }
    }
}
