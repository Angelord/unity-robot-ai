

namespace MrRob {
    public class Tile {

        private Point position;
        private bool blocked;

        public Point Position { get { return position; } }
        public bool Blocked { get { return blocked; } set { blocked = false; } }

        public Tile(Point position) {
            this.position = position;
        }
    }
}