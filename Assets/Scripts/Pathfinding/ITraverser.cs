using System;
namespace MrRob.Pathfinding {
    public interface ITraverser<T> {

        bool CanEndOn(T node);
        bool CanTraverse(T node);
        bool AddToResult(T node);
        bool CanMoveBetween(T start, T end);
        int GetTraverseCost(T start, T end);
    }
}
