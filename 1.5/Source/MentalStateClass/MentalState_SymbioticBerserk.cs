using Verse.AI;

namespace Thek_PSS
{
    public class MentalState_SymbioticBerserk : MentalState
    {
        internal int numberOfAttacksPerformed; // AttackMelee jobs actually given
        internal int? totalAttacks; // Limit of how many AttackMelee jobs can be given (a random number in the intRange)

        public override bool ForceHostileTo(Faction f)
        {
            return true;
        }

        public override bool ForceHostileTo(Thing t)
        {
            return true;
        }

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }
    }
}
