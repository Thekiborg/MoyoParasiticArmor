using System.Linq;

namespace Thek_PSS
{
    public class HediffComp_SymbioticRegeneration : HediffComp_RunOnStage
    {
        public new HediffCompProperties_SymbioticRegeneration Props => (HediffCompProperties_SymbioticRegeneration)props;

        private int? lastTick;
        private int? ticksToRegenInt;

        private int? TicksToRegen
        {
            get
            {
                ticksToRegenInt ??= Props.ticksToPassToRegen.RandomInRange;
                return ticksToRegenInt;
            }
            set { ticksToRegenInt = value; }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Pawn.IsHashIntervalTick(Props.ticksToCheck)) // Every hour
            {
                if (CanRunInHediffStage)
                {
                    lastTick ??= parent.ageTicks;
                    if ((parent.ageTicks - lastTick) >= TicksToRegen)
                    {
                        lastTick = parent.ageTicks;

                        int die = UnityEngine.Random.Range(1, 2);
                        if (die == 1)
                        {
                            TryHealRandomPermanentWound(Pawn, parent.LabelBase);
                        }
                        else
                        {
                            TryHealMissingBodyPart(Pawn, parent.LabelBase);
                        }
                    }
                }
                for (int i = 0; i < Pawn.health.hediffSet.hediffs.Count; i++)
                {
                    if (Pawn.health.hediffSet.hediffs[i].Bleeding)
                    {
                        Pawn.health.hediffSet.hediffs[i].Tended(0.75f, 1);
                    }
                }
            }
        }


        /// <summary>
        /// The method inside luciferium that heals scars and such
        /// </summary>
        public static void TryHealRandomPermanentWound(Pawn pawn, string cause)
        {
            if (pawn.health.hediffSet.hediffs.Where((Hediff hd) => hd.IsPermanent() || hd.def.chronic).TryRandomElement(out var result))
            {
                HealthUtility.Cure(result);
                if (PawnUtility.ShouldSendNotificationAbout(pawn))
                {
                    Messages.Message("MessagePermanentWoundHealed".Translate(cause, pawn.LabelShort, result.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
        }


        public static void TryHealMissingBodyPart(Pawn pawn, string cause)
        {
            List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < allHediffs.Count; i++)
            {
                if (allHediffs[i] is Hediff_MissingPart missingPart)
                {
                    MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, missingPart.Part, pawn.Position, pawn.Map);

                    Messages.Message("PlenamStratusSymbiotica_RegeneratedPartMessage".Translate(cause, pawn.LabelShort, missingPart.Part.Label, pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
                    break;
                }
            }
        }


        public override void CompExposeData()
        {
            Scribe_Values.Look(ref lastTick, "PlenamStratusSymbiotica_HediffComp_SymbioticRegeneration_SelfScratch_lastTick");
            Scribe_Values.Look(ref ticksToRegenInt, "PlenamStratusSymbiotica_HHediffComp_SymbioticRegeneration_ticksToRegenInt");
        }
    }
}
