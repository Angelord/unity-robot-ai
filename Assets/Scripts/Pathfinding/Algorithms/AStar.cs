using System;
using System.Linq;
using System.Collections.Generic;

namespace MrRob.Pathfinding.Algorithms {
    public class AStar<T> : IPathfindingAlgorithm<T> {

        private readonly PathfindingNode<T>[] nodes;
        private readonly int width;
        private readonly int height;

        private Point startPos;
        private Point endPos;
        private ITraverser<T> traverser;
        private List<PathfindingNode<T>> closedList = new List<PathfindingNode<T>>();
        private List<PathfindingNode<T>> openList = new List<PathfindingNode<T>>();
        private PathfindingNode<T> current;

        public AStar(T[] nodeData, int width) {
            this.nodes = new PathfindingNode<T>[nodeData.Length];
            this.width = width;
            this.height = nodes.Length / width;

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    nodes[x + y * width] = new PathfindingNode<T>(nodeData[x + y * width], new Point(x, y));
                }
            }
        }

        public Path GetPath(Point startPos, Point endPos, ITraverser<T> traverser) {
            if(startPos == endPos) {
                return new Path(new List<Point>() { startPos });   
            }

            this.startPos = startPos;
            this.endPos = endPos;
            this.traverser = traverser;
            closedList.Clear();
            openList.Clear();
            GetNode(startPos).GCost = 0;

            openList.Add(GetNode(startPos));

            bool found = false;
            while (!found) {
                if(openList.Count == 0) {
                    return new Path(); //No path exists
                }

                current = openList[openList.Count - 1];
                openList.Remove(current);

                if(current.Position == endPos) {
                    found = true;
                    return Backtrack();
                }


                if(!closedList.Contains(current)) {
                    closedList.Add(current);
                    AddNeighboursToOpen();
                }
            }

            return new Path();
        }

        private Path Backtrack() {
            List<Point> path = new List<Point>();

            while(current.Position != startPos) {
                if(traverser.AddToResult(current.Data)) { 
                    path.Add(current.Position);
                }
                current = current.Parent;
            }

            if(traverser.AddToResult(current.Data)) {
                path.Add(startPos);
            }

            path.Reverse();

            return new Path(path);
        }

        private void AddNeighboursToOpen() {
            foreach(Point neighbour in GridUtility.GetNeighbours(current.Position, width, height)) {
                AddToOpen(neighbour);
            }

            openList.Sort((a, b) => b.FCost.CompareTo(a.FCost));
        }

        private void AddToOpen(Point point) {
            PathfindingNode<T> target = GetNode(point);
            if((traverser.CanMoveBetween(current.Data, target.Data) && traverser.CanTraverse(target.Data)) 
            || point == endPos) {
                int gCost = current.GCost + traverser.GetTraverseCost(current.Data, target.Data);
                if (!closedList.Contains(target) && !openList.Contains(target)) {
                    target.Parent = current;
                    target.HCost = point.GetDistance(endPos);
                    target.GCost = gCost;
                    openList.Add(target);
                }
                else if(target.GCost > gCost) {
                    target.Parent = current;
                    target.GCost = gCost;
                }
            }
        }

        private PathfindingNode<T> GetNode(Point point) {
            return nodes[point.X + point.Y * width];
        }
    }
}
