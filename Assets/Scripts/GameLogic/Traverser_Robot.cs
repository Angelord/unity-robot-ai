using System.Collections.Generic;
using MrRob.Pathfinding;

namespace MrRob.GameLogic {

    public class Traverser_Robot : ITraverser<Tile> {

        private Robot robot;
        private List<Point> fixedBlocks = new List<Point>();
        private bool avoidCargo = true;

        public List<Point> FixedBlocks { get { return fixedBlocks; } set { fixedBlocks = value; } }
        public bool AvoidCargo { get { return avoidCargo; } set { avoidCargo = value; } }

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
            bool canTraverse = robot.CanTraverse(node.Position);
            
            if(canTraverse && (!avoidCargo || robot.Game.Cargo.Position != node.Position)) {
                return true;
            }
            return false;
        }

        public bool CanMoveBetween(Tile start, Tile end) {
            return true;
        }

        public int GetTraverseCost(Tile start, Tile end) {
            return 1;
        }
    }
}