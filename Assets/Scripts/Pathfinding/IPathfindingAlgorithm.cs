using System;

namespace MrRob.Pathfinding {

    public interface IPathfindingAlgorithm<T> {

        Path GetPath(Point startPos, Point endPos, ITraverser<T> traverser);
    }
}
