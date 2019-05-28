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


        public void Prepend(Path other) {
            if(other.End != Start) {
                throw new ArgumentException("To prepend a path its end node must match the target's start node.");
            }

            for(int i = 0; i < other.Length - 1; i++) {
                pathNodes.Insert(0, other[i]);
            }
        }

        public void Append(Path other) {
            if(other.Start != End) {
                throw new ArgumentException("To append a path its start node must match the target's end node.");
            }

            for(int i = 1; i < other.Length; i++) {
                pathNodes.Add(other[i]);
            }
        }
    }

}
