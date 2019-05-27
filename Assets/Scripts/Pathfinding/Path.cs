using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace MrRob.Pathfinding {
    
    public class Path : IEnumerable<Point> {

        private readonly List<Point> pathNodes;

        public Point Start { get { return pathNodes[0]; } }
        public Point End { get { return pathNodes[pathNodes.Count - 1]; } }
        public int Length { get { return pathNodes == null ? 0 : pathNodes.Count; } }
        public bool Exists { get { return pathNodes != null; } }
        public int Count { get { return pathNodes.Count; } }
        public ReadOnlyCollection<Point> Nodes { get { return pathNodes.AsReadOnly(); } }
        public Point this[int index] { get { return pathNodes[index]; } }

        public Path() { }
        public Path(List<Point> pathNodes) { this.pathNodes = pathNodes; }
        public Path(Point[] pathNodes) { this.pathNodes = new List<Point>(pathNodes); }

        public Point GetNode(int index) { return pathNodes[index]; }
        public IEnumerator<Point> GetEnumerator() { return pathNodes.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    }

}
