using MrRob.Pathfinding;

namespace MrRob.GameLogic {

    public class RobotTraverser : ITraverser<Tile> {

        private Robot robot;

        public RobotTraverser(Robot robot) {
            this.robot = robot;
        }

        public bool AddToResult(Tile node) {
            return true;
        }

        public bool CanEndOn(Tile node) {
            return true;
        }

        public bool CanTraverse(Tile node) {
            return (!robot.TileIsRevealed(node.Position) || !node.Blocked);
        }

        public int GetTraverseCost(Tile start, Tile end) {
            return 1;
        }
    }
}