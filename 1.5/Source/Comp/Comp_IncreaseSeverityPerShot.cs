using Verse;

namespace Thek_PSS
{
    public class Comp_IncreaseSeverityPerShot : CompEquippable
    {
        CompProperties_IncreaseSeverityPerShot Props => (CompProperties_IncreaseSeverityPerShot)props;
        public override void Notify_UsedWeapon(Pawn pawn)
        {
            base.Notify_UsedWeapon(pawn);
            Log.Message("Runs");
            HealthUtility.AdjustSeverity(pawn, Props.hediffDefToIncrease, Props.severityIncreasePerShot);
            Log.Message("Healht utility is trolling");
        }
    }
}
