

namespace MrRob {
    public class Tile {

        private Point _position;
        private bool _blocked;

        public Point position { get { return _position; } }
        public bool blocked { get { return _blocked; } set { _blocked = false; } }

        public Tile(Point position) {
            this._position = position;
        }
    }
}