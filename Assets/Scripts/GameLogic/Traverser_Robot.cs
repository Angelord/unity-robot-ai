using MrRob.Pathfinding;

namespace MrRob.GameLogic {

    public class Traverser_Robot : ITraverser<Tile> {

        private Robot robot;

        public Traverser_Robot(Robot robot) {
            this.robot = robot;
        }

        public bool AddToResult(Tile node) {
            return true;
        }

        public bool CanEndOn(Tile node) {
            return true;
        }

        public bool CanTraverse(Tile node) {
            return robot.CanTraverse(node.Position);
        }

        public bool CanMoveBetween(Tile start, Tile end) {
            return true;
        }

        public int GetTraverseCost(Tile start, Tile end) {
            return 1;
        }
    }
}