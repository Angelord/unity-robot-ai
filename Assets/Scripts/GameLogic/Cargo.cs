namespace MrRob.GameLogic {
    public class Cargo {

        private RobotGame game;

        public Point Position { get; set; }

        public Cargo(RobotGame game) { 
            this.game = game;
        }

        public bool CanPush(Point direction) {
            Point newPos = Position + direction;
            if(!game.Contains(newPos)) { return false; }

            return !game.GetTile(newPos).Blocked; 
        }

        public void Push(Point direction) {
            if(!CanPush(direction)) {
                throw new System.InvalidOperationException(string.Format("Invalid push direction for cargo {0}", direction));
            }

            Position = Position + direction;
        }
    }
}