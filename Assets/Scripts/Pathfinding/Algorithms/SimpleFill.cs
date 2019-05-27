using System;
using System.Collections.Generic;

namespace MrRob.Pathfinding.Algorithms {
    public class SimpleFill<T> : IFillAlgorithm<T> {

        private readonly FillNode<T>[] nodes;
        private readonly int width;
        private readonly int length;

        private readonly List<FillNode<T>> fill = new List<FillNode<T>>();
        private readonly List<FillNode<T>> frontier = new List<FillNode<T>>();

        private ITraverser<T> traverser;

        public SimpleFill(T[] data, int width) {
            nodes = new FillNode<T>[data.Length];
            this.width = width;
            this.length = data.Length / width;

            for(int x = 0; x < width; x++) { 
                for(int y = 0; y < length; y++) {
                    nodes[x + y * width] = new FillNode<T>(data[x + y * width], new Point(x, y));   
                }
            }
        }

        public Area GetFill(Point center, int range, ITraverser<T> traverser) {
            this.traverser = traverser;

            fill.Clear();
            frontier.Clear();
            foreach(FillNode<T> node in nodes) { node.Frontier = false; } 

            FillNode<T> first = GetNode(center);
            first.GCost = 0;
            frontier.Add(GetNode(center));
            first.Frontier = true;;

            for(int step = 0; step <= range; step++) {
                for(int i = frontier.Count - 1; i >= 0; i--) {
                    if(frontier[i].GCost <= step) {
                        fill.Add(frontier[i]);
                        AddNeighbours(frontier[i]);
                        frontier.RemoveAt(i);
                    }
                }
            }

            List<Point> fillResult = new List<Point>();
            for(int i = 0; i < fill.Count; i++) {
                if(traverser.CanEndOn(fill[i].Data) && traverser.AddToResult(fill[i].Data)) {
                    fillResult.Add(fill[i].Position);
                }
            }

            return new Area(fillResult);
        }

        private void AddNeighbours(FillNode<T> node) {
            List<Point> neighbours = GridUtility.GetNeighbours(node.Position, width, length);
            foreach(Point neighbour in neighbours) {
                FillNode<T> neighNode = GetNode(neighbour);
                if(!neighNode.Frontier && traverser.CanTraverse(neighNode.Data)) {
                    neighNode.GCost = node.GCost + traverser.GetTraverseCost(node.Data, neighNode.Data);
                    frontier.Add(neighNode);
                    neighNode.Frontier = true;
                }
            }
        }

        private FillNode<T> GetNode(Point pos) {
            return nodes[pos.X + pos.Y * width];
        }
    }
}
