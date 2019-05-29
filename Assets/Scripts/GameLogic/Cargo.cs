namespace MrRob.GameLogic {
    public class Cargo {

        private RobotGame game;
        private Point startingPos;
        private Point curPos;

        public Point Position { 
            get { return curPos; } 
            set { startingPos = value; curPos = value; } 
        }

        public Cargo(RobotGame game) { 
            this.game = game;
        }

        public void Reset() {
            curPos = startingPos;
        }

        public bool CanPush(Point direction) {
            Point newPos = curPos + direction;
            if(!game.Contains(newPos)) { return false; }

            return !game.GetTile(newPos).Blocked; 
        }

        public void Push(Point direction) {
            if(!CanPush(direction)) {
                throw new System.InvalidOperationException(string.Format("Invalid push direction for cargo {0}", direction));
            }
            curPos = curPos + direction;
        }
    }
}