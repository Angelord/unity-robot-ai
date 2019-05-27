using System.Collections.Generic;

namespace MrRob.GameLogic {
    public class Robot {

        private RobotGame map;
        private Dictionary<string, RobotState> states = new Dictionary<string, RobotState>();
        private bool treasureFound;
        private bool[] tilesRevealed;
        private Point position;

        public RobotGame Map { get { return map; } }
        public Point Position { get { return position; } }
        public bool TreasureFound { get { return treasureFound; } }

        public Robot(RobotGame map) {
            this.map = map;
            this.position = Point.ZERO;
            tilesRevealed = new bool[map.Width * map.Length];
        }

        public void SetTileRevealed(Point pos) {
            tilesRevealed[pos.X + pos.Y * map.Width] = true;
        } 

        public bool TileIsRevealed(Point pos) {
            return tilesRevealed[pos.X + pos.Y * map.Width];
        }
    
        public void EnterState(string stateName) {
            throw new System.NotImplementedException();
        }
    }
}