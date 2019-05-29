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
            return robot.CanTraverse(node.Position);
        }

        public int GetTraverseCost(Tile start, Tile end) {
            if(!robot.TileIsRevealed(end.Position)) {
                return 2;
            }
            return 1;
        }
    }
}