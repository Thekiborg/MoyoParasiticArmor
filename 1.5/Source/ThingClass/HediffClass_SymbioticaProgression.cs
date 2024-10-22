using Verse;

namespace Thek_PSS
{
    public class HediffClass_SymbioticaProgression : HediffWithComps
    {
        private PlenamStratusSymbiotica_ModExtension modExtInt;
        private bool isWildInt;
        private GameComponent_PlenamStratusSymbiotica gameComp;


        public GameComponent_PlenamStratusSymbiotica GameComp
        {
            get
            {
                gameComp ??= Current.Game.GetComponent<GameComponent_PlenamStratusSymbiotica>();
                return gameComp;
            }
        }
        public PlenamStratusSymbiotica_ModExtension ModExt
        {
            get
            {
                modExtInt ??= def.GetModExtension<PlenamStratusSymbiotica_ModExtension>();
                return modExtInt;
            }
        }

        public bool IsWild
        {
            get { return isWildInt; }
            set { isWildInt = value; }
        }


        public override HediffStage CurStage
        {
            get
            {
                if (IsWild)
                {
                    return base.CurStage;
                }
                else
                {
                    if (!ModExt.controlledSymbionteStages.NullOrEmpty())
                    {
                        return ModExt.controlledSymbionteStages[CurStageIndex];
                    }
                    return null;
                }
            }
        }


        public override int CurStageIndex
        {
            get
            {
                if (IsWild)
                {
                    return base.CurStageIndex;
                }
                else
                {
                    if (ModExt.controlledSymbionteStages == null)
                    {
                        return 0;
                    }
                    for (int num = ModExt.controlledSymbionteStages.Count -1; num >= 0; num--)
                    {
                        if (Severity >= ModExt.controlledSymbionteStages[num].minSeverity)
                        {
                            return num;
                        }
                    }
                    return 0;
                }
            }
        }


        protected override void OnStageIndexChanged(int stageIndex)
        {
            base.OnStageIndexChanged(stageIndex);

            // If the list is empty the method won't run
            if (ModExt.SymbioticaOnStageIndexChangedActions.NullOrEmpty()) return;

            for (int i = 0; i < ModExt.SymbioticaOnStageIndexChangedActions.Count; i++)
            {
                var onIndexChangedActions = ModExt.SymbioticaOnStageIndexChangedActions[i];

                if (IsWild && !onIndexChangedActions.happensOnWild || !IsWild && !onIndexChangedActions.happensOnControlled) continue;
                /*
                 * If the hediff is wild but the action on this index doesn't happen when it's wild
                 * or the hediff is controlled but the action on this index doesn't happen when it's controlled
                 * we jump to the next action and repeat this check for every action on the XML.
                 */

                if (stageIndex == onIndexChangedActions.stageIndex)
                {
                    if (onIndexChangedActions.sendsLetter)
                    {
                        Pawn highestMedicinePawn = PlenamStatusSymbiotica_Utility.GetHighestMedicalNotInfectedPawn(pawn);
                        if (!GameComp.letterBodiesSent.Contains(onIndexChangedActions.letterAndMessageConfig.letterBody)) // If this letter hasn't had been sent before
                        {
                            var letterLabel = onIndexChangedActions.letterAndMessageConfig.letterTitle.Translate(pawn.Named("PAWN"), highestMedicinePawn.Named("DOCTOR"));
                            var letterBody = onIndexChangedActions.letterAndMessageConfig.letterBody.Translate(pawn.Named("PAWN"), highestMedicinePawn.Named("DOCTOR"));

                            Find.LetterStack.ReceiveLetter(letterLabel, letterBody, PlenamStratusSymbioticaLetterDefOfs.Thek_PlenamStratusSymbiotica_SymbioticaLetter, pawn);
                            GameComp.letterBodiesSent.Add(onIndexChangedActions.letterAndMessageConfig.letterBody); // Now it has
                        }
                        else
                        {
                            var messageContent = onIndexChangedActions.letterAndMessageConfig.messageContent.Translate(pawn.Named("PAWN"), highestMedicinePawn.Named("DOCTOR"));

                            Messages.Message(messageContent, MessageTypeDefOf.NeutralEvent);
                        }
                    }


                    if (onIndexChangedActions.appliesSet)
                    {
                        GrowArmor(onIndexChangedActions.grownSet.listOfApparelToGrow, onIndexChangedActions.grownSet.weaponToGrow);
                    }
                }
            }
        }

        private void GrowArmor(List<ThingDef> apparelsToGrow, ThingDef weaponToGrow)
        {
            if (!apparelsToGrow.Empty())
            {
                foreach (var apparelDef in apparelsToGrow)
                {
                    Thing apparelToWear = ThingMaker.MakeThing(apparelDef);
                    if (apparelToWear is Apparel apparel)
                    {
                        pawn.apparel.Wear(apparel, true, true);
                    }
                }
            }

            if (weaponToGrow is not null)
            {
                Thing weaponToEquip = ThingMaker.MakeThing(weaponToGrow);
                if (weaponToEquip is ThingWithComps weapon)
                {
                    pawn.equipment.MakeRoomFor(weapon);
                    pawn.equipment.AddEquipment(weapon);
                }
            }
        }
    }
}
