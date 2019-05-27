using System;
using MrRob.Pathfinding.Algorithms;

namespace MrRob.Pathfinding {
    public class Pathfinder<T> {

        private IPathfindingAlgorithm<T> pathfindingAlgorithm;
        private IFillAlgorithm<T> fillAlgorithm;

        public Pathfinder(T[] gridNodes, int width) {
            pathfindingAlgorithm = new AStar<T>(gridNodes, width);
            fillAlgorithm = new SimpleFill<T>(gridNodes, width);
        }

        public Path GetPath(Point start, Point end, ITraverser<T> traverser) {
            return pathfindingAlgorithm.GetPath(start, end, traverser);
        }

        public Area GetArea(Point center, int range, ITraverser<T> traverser) {
            return fillAlgorithm.GetFill(center, range, traverser);
        }
    }
}
