namespace Thek_PSS
{
#pragma warning disable CA1051
    public class PlenamStratusSymbiotica_ModExtension : DefModExtension
    {
        /* Harmony patches */

        public bool isGrownEquipment; // Marks apparel and equipment for harmony patches to run. Stage 5 is the stage when the armor and equipment are forced on the pawn.


        /* ThinkNode_ChancePerCustomTicks */

        public float mtbHours; // Every how often does the thinkNode try to run
        public float runInterval; // Every how often does the thinkNode check actually run


        /* JobGiver_SymbioticBerserk */

        public IntRange numberOfAttacks; // How many attacks will the pawn attempt per mental state


        /* HediffStages for the controlled version of the hediff */

        public List<HediffStage> controlledSymbionteStages;


        /* Actions to fire when the hediff changes index */
        public List<SymbioticaOnStageIndexChangedActions> SymbioticaOnStageIndexChangedActions;
    }

    /// <summary>
    /// Settings for the letters that get sent when the symbiotica hediff changes stage
    /// </summary>
    public class SymbioticaOnStageIndexChangedActions
    {
        public int stageIndex;
        public bool happensOnWild;
        public bool happensOnControlled;
        public bool appliesSet;
        public bool sendsLetter;
        public GrownSet grownSet;
        public LetterAndMessageConfig letterAndMessageConfig;
    }

    /// <summary>
    /// Fields to force weapons and armor via the hediff
    /// </summary>
    public class GrownSet
    {
        /* Armor and gun to give without hardcoding */

        public List<ThingDef> listOfApparelToGrow;
        public ThingDef weaponToGrow;
    }

    /// <summary>
    /// Fields related to the configuration of the letters and messages sent by the hediff
    /// </summary>
    public class LetterAndMessageConfig
    {
        public string letterBody;
        public string letterTitle = "Thek_PlenamStratusSymbiotica_SymbioticaInjectedLetter_Label";
        public string messageContent = "Thek_PlenamStratusSymbiotica_ProgressedStageMessage";
    }
}