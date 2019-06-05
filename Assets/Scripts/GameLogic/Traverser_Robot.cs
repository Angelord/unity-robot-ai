using System.Collections.Generic;
using MrRob.Pathfinding;
using UnityEngine;

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
            if(fixedBlocks.Contains(node.Position)) {
                return false; 
            }

            if(!robot.TileIsRevealed(node.Position)) {
                return true;
            }

            if(node.Blocked) {
                return false;
            }

            if(avoidCargo && robot.Game.Cargo.Position == node.Position) {
                return false;
            }

            return true;
        }

        public bool CanMoveBetween(Tile start, Tile end) {
            return true;
        }

        public int GetTraverseCost(Tile start, Tile end) {
            return !robot.TileIsRevealed(end.Position) ? 1 : 2;
        }
    }
}