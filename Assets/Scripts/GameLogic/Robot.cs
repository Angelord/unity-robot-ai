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
        private bool done;
        private RobotTraverser traverser;

        public RobotGame Map { get { return map; } }
        public Point Position { get { return position; } }
        public Point Orientation { get { return orientation; } }
        public RobotTraverser Traverser { get { return traverser; } }
        public bool TreasureFound { get { return treasureFound; } }
        public bool Done { get { return done; } set { done = value; } }

        public Robot(RobotGame map) {
            this.map = map;
            this.position = Point.ZERO;
            this.orientation = Point.UP;
            this.traverser = new RobotTraverser(this);

            tilesRevealed = new bool[map.Width * map.Length];
            SetTileRevealed(Point.ZERO);
            SetTileRevealed(map.GoalPosition);

            states.Add("Searching", new State_Searching(this));
            states.Add("Pushing", new State_Pushing(this));
            states.Add("Done", new State_Done(this));

            EnterState("Searching");
        }

        public void Step() {
            if(curState != null) {
                curState.Step();
            }
        }

        public bool TryMove(Point newPos) {
            if(!map.Contains(newPos)) {
                throw new System.ArgumentException(string.Format("Move position {0} is outside map bounds!", newPos));
            }

            if(map.GetTile(newPos).Blocked) {
                SetTileRevealed(newPos);
                return false;
            }

            //TODO : Check for cargo

            position = newPos;
            SetTileRevealed(newPos);
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