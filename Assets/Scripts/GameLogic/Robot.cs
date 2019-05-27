using System.Collections.Generic;

namespace MrRob.GameLogic {
    public class Robot {

        private RobotGame map;
        private Point position;
        private Point orientation;
        private bool[] tilesRevealed;
        private bool treasureFound;
        private Dictionary<string, RobotState> states = new Dictionary<string, RobotState>();
        private RobotState curState = null;

        public RobotGame Map { get { return map; } }
        public Point Position { get { return position; } }
        public Point Orientation { get { return orientation; } }
        public bool TreasureFound { get { return treasureFound; } }

        public Robot(RobotGame map) {
            this.map = map;
            this.position = Point.ZERO;
            this.orientation = Point.UP;

            tilesRevealed = new bool[map.Width * map.Length];
            SetTileRevealed(map.GoalPosition);

            states.Add("Searching", new State_Searching(this));

            EnterState("Searching");
        }

        public bool TryMove(Point newPos) {

            if(newPos.X < 0 || newPos.Y < 0 || newPos.X >= map.Width || newPos.Y >= map.Length) {
                return false;
            }

            if(map.GetTile(newPos).Blocked) {
                SetTileRevealed(newPos);
                return false;
            }

            //TODO : Check for cargo
            
            position = newPos;
            return true;
        }

        public void Look(Point direction) {
            orientation = direction;
        }

        public void SetTileRevealed(Point pos) {
            tilesRevealed[pos.X + pos.Y * map.Width] = true;
        } 

        public bool TileIsRevealed(Point pos) {
            return tilesRevealed[pos.X + pos.Y * map.Width];
        }
    
        public void EnterState(string stateName) {

            if(!states.ContainsKey(stateName)) {
                throw new System.ArgumentException(string.Format("No state with name {} exists for the robot!", stateName));
            }

            if(curState == states[stateName]) { return; }

            if(curState != null) { curState.OnExit(); }

            curState = states[stateName];
            curState.OnEnter();
        }
    }
}