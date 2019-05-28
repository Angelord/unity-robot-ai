using System.Collections.Generic;
using MrRob.Pathfinding;

namespace MrRob.GameLogic {

    public class Traverser_Robot : ITraverser<Tile> {

        private Robot robot;
        private List<Point> fixedBlocks = new List<Point>();

        public List<Point> FixedBlocks { get { return fixedBlocks; } set { fixedBlocks = value; } }

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
            if(fixedBlocks.Contains(node.Position)) { return false; }
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