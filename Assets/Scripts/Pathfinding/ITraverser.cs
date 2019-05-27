﻿using System;
namespace MrRob.Pathfinding {
    public interface ITraverser<T> {

        bool CanEndOn(T node);
        bool CanTraverse(T node);
        bool AddToResult(T node);
        int GetTraverseCost(T start, T end);
    }
}
