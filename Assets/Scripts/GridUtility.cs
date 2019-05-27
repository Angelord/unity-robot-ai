using System;
using System.Collections.Generic;

namespace MrRob {
    public static class GridUtility {

        public static List<Point> GetNeighbours(Point point, int gridWidth, int gridLength) {
            List<Point> neighbours = new List<Point>();
            if(point.X > 0) { neighbours.Add(new Point(point.X - 1, point.Y)); }
            if(point.Y > 0) { neighbours.Add(new Point(point.X, point.Y - 1)); }
            if(point.X < gridWidth - 1) { neighbours.Add(new Point(point.X + 1, point.Y)); }
            if(point.Y < gridLength - 1) { neighbours.Add(new Point(point.X, point.Y + 1)); }
            return neighbours;
        }

        //GetCircle()
    }
}
