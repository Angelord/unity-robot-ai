using System.Collections.Generic;
using MrRob.Pathfinding;
using MrRob.Pathfinding.Algorithms;

namespace MrRob.GameLogic {
    public class Robot {

        private RobotGame map;
        private Point position;
        private Point orientation;
        private bool[] tilesRevealed;
        private bool treasureFound;
        private Dictionary<string, RobotState> states = new Dictionary<string, RobotState>();
        private RobotState prevState = null;
        private RobotState curState = null;
        private IPathfindingAlgorithm<Tile> pathfinding;
        private RobotTraverser traverser;
        private bool done;

        public RobotGame Map { get { return map; } }
        public Point Position { get { return position; } }
        public Point Orientation { get { return orientation; } }
        public IPathfindingAlgorithm<Tile> Pathfinding { get { return pathfinding; } }
        public RobotTraverser Traverser { get { return traverser; } }
        public bool TreasureFound { get { return treasureFound; } }
        public bool Done { get { return done; } set { done = value; } }

        public Robot(RobotGame game) {
            this.map = game;
            this.position = Point.ZERO;
            this.orientation = Point.UP;
            this.pathfinding = new AStar<Tile>(game.Tiles, game.Width);
            this.traverser = new RobotTraverser(this);

            tilesRevealed = new bool[game.Width * game.Length];
            SetTileRevealed(Point.ZERO);
            SetTileRevealed(game.GoalPosition);

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

            EnterState(states[stateName]);
        }

        public void EnterState(RobotState state) {
            if(curState == state) { return; }

            if(curState != null) { curState.OnExit(); }

            prevState = curState;
            curState = state;
            curState.OnEnter();
        }

        public void ReturnToPrevState() {
            EnterState(prevState);
        }
    }
}