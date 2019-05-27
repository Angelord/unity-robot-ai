using System;
using System.Collections.Generic;

namespace MrRob {
    public static class GridUtility {

        public static List<Point> GetNeighbours(Point point, int gridWidth, int gridLength) {
            List<Point> neighbours = new List<Point>();
            if(point.x > 0) { neighbours.Add(new Point(point.x - 1, point.y)); }
            if(point.y > 0) { neighbours.Add(new Point(point.x, point.y - 1)); }
            if(point.x < gridWidth - 1) { neighbours.Add(new Point(point.x + 1, point.y)); }
            if(point.y < gridLength - 1) { neighbours.Add(new Point(point.x, point.y + 1)); }
            return neighbours;
        }

        //GetCircle()
    }
}
