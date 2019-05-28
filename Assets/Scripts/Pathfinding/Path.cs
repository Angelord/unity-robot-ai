using System;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MrRob.Pathfinding {
    
    public class Path : IEnumerable<Point> {

        private List<Point> pathNodes = new List<Point>();

        public Point Start { get { return pathNodes[0]; } }
        public Point End { get { return pathNodes[pathNodes.Count - 1]; } }
        public int Length { get { return pathNodes == null ? 0 : pathNodes.Count; } }
        public bool Exists { get { return pathNodes != null && pathNodes.Count != 0; } }
        public int Count { get { return pathNodes.Count; } }
        public ReadOnlyCollection<Point> Nodes { get { return pathNodes.AsReadOnly(); } }
        public Point this[int index] { get { return pathNodes[index]; } }

        public Path() { this.pathNodes = new List<Point>(); }
        public Path(List<Point> pathNodes) { this.pathNodes = pathNodes; }
        public Path(Point[] pathNodes) { this.pathNodes = new List<Point>(pathNodes); }

        public Point GetNode(int index) { return pathNodes[index]; }
        public IEnumerator<Point> GetEnumerator() { return pathNodes.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    
        public void Prepend(Path other) {
            if(other.End != Start) {
                throw new ArgumentException("To prepend a path its end node must match the target's start node.");
            }
            pathNodes.RemoveAt(0);
            pathNodes = other.Concat(pathNodes).ToList();
        }

        public void Append(Path other) {
            if(other.Start != End) {
                throw new ArgumentException("To append a path its start node must match the target's end node.");
            }

            pathNodes.RemoveAt(pathNodes.Count - 1);
            pathNodes = pathNodes.Concat(other).ToList();
        }

        public void Append(Point pos) {
            pathNodes.Add(pos);
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < Length; i++) {
                builder.Append("->");
                builder.Append(pathNodes[i]);
            }
            return builder.ToString();
        }
    }

}
