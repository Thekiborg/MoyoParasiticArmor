namespace Thek_PSS
{
    public class ThingClass_SymbioticaApparel : Apparel
    {
        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            pawn.apparel?.Lock(this);
        }
    }
}
