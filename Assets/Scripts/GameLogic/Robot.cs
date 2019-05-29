using System.Collections.Generic;
using MrRob.Pathfinding;
using MrRob.Pathfinding.Algorithms;

namespace MrRob.GameLogic {
    public class Robot {

        private RobotGame game;
        private Point position;
        private Point orientation;
        private bool[] tilesRevealed;
        private bool cargoFound;
        private Dictionary<string, RobotState> states = new Dictionary<string, RobotState>();
        private RobotState prevState = null;
        private RobotState curState = null;
        private IPathfindingAlgorithm<Tile> pathfinding;
        private Traverser_Robot traverser;
        private bool done = false;
        private string resultMsg;

        public RobotGame Game { get { return game; } }
        public Point Position { get { return position; } }
        public Point Orientation { get { return orientation; } }
        public IPathfindingAlgorithm<Tile> Pathfinding { get { return pathfinding; } }
        public Traverser_Robot Traverser { get { return traverser; } }
        public bool CargoFound { get { return cargoFound; } }
        public bool Done { get { return done; } }
        public string ResultMsg { get { return resultMsg; } }

        public Robot(RobotGame game) {
            this.game = game;
            this.position = Point.ZERO;
            this.orientation = Point.UP;
            this.pathfinding = new AStar<Tile>(game.Tiles, game.Width);
            this.traverser = new Traverser_Robot(this);

            tilesRevealed = new bool[game.Width * game.Length];

            states.Add("Searching", new State_Searching(this));
            states.Add("Pushing", new State_Pushing(this));
            states.Add("Idle", new State_Idle(this));
        
            EnterState("Idle");
        }

        public void Init() {
            SetTileRevealed(Point.ZERO);
            SetTileRevealed(game.GoalPosition);

            EnterState("Searching");
        }

        public void Step() {
            if(curState != null) {
                curState.Step();
            }
        }

        public void SetDone(string msg) {
            if(!done) {
                done = true;
                resultMsg = msg;
                EnterState("Idle");
            }
        }
 
        public void Look(Point direction) {
            orientation = direction;
        }
        
        public bool TryMove() {
            Point newPos = position + orientation;

            if(!game.Contains(newPos)) {
                throw new System.ArgumentException(string.Format("Move position {0} is outside map bounds!", newPos));
            }

            if(newPos == game.Cargo.Position) {
                return TryPushCargo();
            }

            if(game.GetTile(newPos).Blocked) {
                SetTileRevealed(newPos);
                return false;
            }

            position = newPos;
            SetTileRevealed(newPos);
            return true;
        }

        public bool CanTraverse(Point pos) { //Determines if the robot can step on a tile based on current information
            return  game.Contains(pos) &&
                    (!TileIsRevealed(pos) || (!game.GetTile(pos).Blocked && game.Cargo.Position != pos)); 
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

        public bool TileIsRevealed(Point pos) {
            return tilesRevealed[pos.X + pos.Y * game.Width];
        }

        private void SetTileRevealed(Point pos) {
            tilesRevealed[pos.X + pos.Y * game.Width] = true;
            if(pos == game.Cargo.Position) {
                cargoFound = true;
            }
        } 

        private bool TryPushCargo() {
            Point newPos = position + orientation;

            SetTileRevealed(newPos);

            Point newCargoPos = game.Cargo.Position + orientation;
            if(game.Contains(newCargoPos)) {
                SetTileRevealed(newCargoPos);
            }

            if(game.Cargo.CanPush(orientation)) {
                game.Cargo.Push(orientation);
                position = newPos;
                return true;
            }
            else {
                return false;
            }
        }
    }
}