using MrRob.Pathfinding;

namespace MrRob.GameLogic {

    public class Traverser_Cargo : ITraverser<Tile> {
        
        private Robot robot;

        public Traverser_Cargo(Robot robot) {
            this.robot = robot;
        }

        public bool AddToResult(Tile node) {
            return true;
        }

        public bool CanEndOn(Tile node) {
            return true;
        }

        public bool CanMoveBetween(Tile start, Tile end) {
            Point moveDir = end.Position - start.Position;
            Point prevPos = start.Position - moveDir;

            return robot.CanTraverse(prevPos);
        }

        public bool CanTraverse(Tile node) {
            throw new System.NotImplementedException();
        }

        public int GetTraverseCost(Tile start, Tile end)
        {
            throw new System.NotImplementedException();
        }
    }
}