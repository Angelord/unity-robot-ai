using System;

namespace MrRob.Pathfinding {
    public interface IFillAlgorithm<T> {
        Area GetFill(Point center, int range, ITraverser<T> traverser);
    }
}
