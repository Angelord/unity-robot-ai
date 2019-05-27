using System;

namespace MrRob.Pathfinding.Algorithms {
    public class PathfindingNode<T> {

        private readonly T data;
        private readonly Point position;
        private PathfindingNode<T> parent;
        private int hCost;
        private int gCost;

        public T Data { get { return data; } }
		public Point Position { get { return position; } }
        public PathfindingNode<T> Parent { get { return parent; } set { parent = value; } }
        public int HCost { get { return hCost; } set { hCost = value; } }
        public int GCost { get { return gCost; } set { gCost = value; } }
        public int FCost { get { return hCost + gCost; } }

        public PathfindingNode(T data, Point position) {
            this.data = data;
            this.position = position;
        }
    }
}
