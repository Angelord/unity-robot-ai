using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MrRob.Pathfinding {
    public class Area : IEnumerable<Point> {

        private readonly List<Point> area = new List<Point>();

        public int Count { get { return area.Count; } }
        public Point this[int index] { get { return area[index]; } }

        public Area(Point[] area) { this.area = new List<Point>(area); }
        public Area(List<Point> area) { this.area = area; }

        public IEnumerator<Point> GetEnumerator() { return area.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return area.GetEnumerator(); }
        public ReadOnlyCollection<Point> GetPoints() { return area.AsReadOnly(); }
        public bool Contains(Point pos) { return area.Contains(pos); }
    }
}
