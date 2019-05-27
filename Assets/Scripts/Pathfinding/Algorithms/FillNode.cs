using System;

namespace MrRob.Pathfinding.Algorithms {
    public class FillNode<T> {

        private readonly T data;
        private readonly Point position;
        private int gCost;
        private bool frontier;

        public T Data { get { return data; } }
        public Point Position { get { return position; } }
        public int GCost { get { return gCost; } set { gCost = value; } }
        public bool Frontier { get { return frontier; } set { frontier = value; }}

        public FillNode(T data, Point position) {
            this.data = data;
            this.position = position;
        }
    }
}
