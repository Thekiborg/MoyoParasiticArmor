using Verse.AI;

namespace Thek_PSS
{
    public class JobGiver_SymbioticWander : JobGiver_WanderAnywhere
    {
        /// <summary>
        /// Adds a check for the job to not run if the pawn is doing specific jobs before it, so it doesn't switch back and forth between them.
        /// </summary>
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.CurJobDef == JobDefOf.AttackMelee)
            {
                return null;
            }
            return base.TryGiveJob(pawn);
        }

        /// <summary>
        /// Tries to get the pawn to wander in usually populated areas, found using the GatheringWorker_Party's cell detection.
        /// </summary>
        protected override IntVec3 GetWanderRoot(Pawn pawn)
        {
            Building spot = pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.PartySpot).RandomElement();
            if (spot != null)
            {
                return spot.Position;
            }
            Room room = pawn.Map.regionGrid.allRooms.Find(room => room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10);
            if (room != null)
            {
                return room.Cells.RandomElement();
            }
            return base.GetWanderRoot(pawn);
        }
    }
}
