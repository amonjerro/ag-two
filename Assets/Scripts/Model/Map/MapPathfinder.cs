using SaveGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExplorationMap
{
    public class PathfindingNode
    {
        
        public int X;
        public int Y;

        public (int, int) Loc { get { return (X, Y); } }

        public int estimatedCost;
        public int actualCost;
        public PathfindingNode parent = null;

        public PathfindingNode(int X, int Y, int Estimated, int Actual)
        {
            this.X = X;
            this.Y = Y;
            estimatedCost = Estimated;
            actualCost = Actual;
        }
    }
    public class MapPathfinder
    {

        private class PriorityQueue
        {
            List<PathfindingNode> nodeHeap;

            public PriorityQueue()
            {
                nodeHeap = new List<PathfindingNode>();
            }

            public PathfindingNode Peek()
            {
                if (nodeHeap.Count == 0) {
                    return null;
                }
                return nodeHeap[0];
            }

            public void Enqueue(PathfindingNode node) { 
                nodeHeap.Add(node);
                HeapifyUp(Count - 1);
            }
            public PathfindingNode Dequeue() { 
                if (nodeHeap.Count == 0) throw new InvalidOperationException("Pathfinding heap is empty.");

                PathfindingNode minValue = nodeHeap[0];
                nodeHeap[0] = nodeHeap[nodeHeap.Count - 1];
                nodeHeap.RemoveAt(nodeHeap.Count - 1);
                HeapifyDown(0);
                return minValue;
            }

            public void Update((int, int) coordinates, int newActualCost, int distance)
            {
                int index = 0;

                // Find the item
                for (int i = 0; i < nodeHeap.Count; i++)
                {
                    if (coordinates == nodeHeap[i].Loc)
                    {
                        break;
                    }
                    index++;
                }
                nodeHeap[index].actualCost = newActualCost;
                nodeHeap[index].estimatedCost = nodeHeap[index].actualCost + distance;
                HeapifyUp(index);

            }
            
            public int Count => nodeHeap.Count;

            private void Swap(int index1, int index2)
            {
                PathfindingNode tmp = nodeHeap[index1];
                nodeHeap[index1] = nodeHeap[index2];
                nodeHeap[index2] = tmp;
            }

            private void HeapifyDown(int index)
            {
                while(index < Count)
                {
                    int leftChildIndex = index * 2 + 1;
                    int rightChildIndex = index * 2 + 2;
                    int smallestIndex = index;
                    if (leftChildIndex < nodeHeap.Count && nodeHeap[leftChildIndex].estimatedCost < nodeHeap[smallestIndex].estimatedCost)
                    {
                        smallestIndex = leftChildIndex;
                    }
                    if (rightChildIndex < nodeHeap.Count && nodeHeap[rightChildIndex].estimatedCost < nodeHeap[smallestIndex].estimatedCost)
                    {
                        smallestIndex = rightChildIndex;
                    }

                    if (smallestIndex == index)
                    {
                        break;
                    }

                    Swap(index, smallestIndex);
                    index = smallestIndex;
                }
            }

            private void HeapifyUp(int index)
            {
                while (index > 0)
                {
                    int parentIndex = (index - 1) / 2;
                    if (nodeHeap[index].estimatedCost >= nodeHeap[parentIndex].estimatedCost)
                    {
                        break;
                    }

                    Swap(index, parentIndex);
                    index = parentIndex;
                }
            }

        }


        ExplorationMap explorationMap;
        TileMovementSO tileCosts;
        public MapPathfinder(TileMovementSO movementData, ExplorationMap map) { 
            tileCosts = movementData;
            tileCosts.SetUpTileTypeData();
            explorationMap = map;
        }

        public float GetTerrainProbability(ConnectionType type)
        {
            return tileCosts.GetOdds(type);
        }

        // Fancy lookup with origin
        private PathfindingNode FindPath((int, int) destination, (int, int) origin)
        {
            // Find the path
            MapTile currentTile;
            PathfindingNode node = new PathfindingNode(origin.Item1, origin.Item2, GetDistance(origin, destination), 0);
            PriorityQueue queue = new PriorityQueue();
            queue.Enqueue(node);
            int candidateX;
            int candidateY;
            
            HashSet<(int,int)> visited = new HashSet<(int,int)>();
            Dictionary<(int,int), PathfindingNode> open = new Dictionary<(int,int), PathfindingNode>();
            PathfindingNode current = node;
            while (queue.Count > 0)
            {
                current = queue.Dequeue();
                open.Remove(current.Loc);
                currentTile = explorationMap.GetTile(current.Loc);
                visited.Add(current.Loc);

                // We have found our destination.
                // This is the only valid escape
                if (current.Loc == destination) {
                    return current;
                }

                for (int i = 0; i < 4; i++)
                {
                    // Syntactic sugar
                    int traversalCost = tileCosts.tileTypeData[currentTile.connections[i]].value;
                    int candidateCost = current.actualCost + traversalCost;
                    PathfindingNode newNode;

                    // Math finagling to change a single index value into +1/-1 lookups in two dimensions clockwise
                    candidateX = (2 - i) * (i % 2);
                    candidateY = (1 - i) * ((1 + i) % 2);
                    (int, int) candidateLoc = (current.Loc.Item1 + candidateX, current.Loc.Item2 + candidateY);
                    bool candidateIsEnd = candidateLoc == destination;

                    // Validate that neighbors can be enqueued at all
                    if ((!candidateIsEnd && !IsValidPathOption(candidateLoc, currentTile.connections[i])) || visited.Contains(candidateLoc))
                    {
                        continue;
                    }

                    // Check to see if this candidate is already in the open list
                    if (open.ContainsKey(candidateLoc))
                    {
                        int distance = GetDistance(candidateLoc, destination) ;
                        int newActual = candidateCost;
                        if (distance + newActual < open[candidateLoc].estimatedCost) { 
                            queue.Update(candidateLoc, newActual, distance);
                            open[candidateLoc].parent = current;
                        }
                        continue;
                    }

                    newNode = new PathfindingNode(
                            candidateLoc.Item1, candidateLoc.Item2,
                            GetDistance(candidateLoc, destination) + candidateCost,
                            candidateCost);
                    newNode.parent = current;
                    queue.Enqueue(newNode);
                    open.Add(candidateLoc, newNode);
                }
            }

            // No path found, we return nothing
            return null;
        }



        public Stack<PathfindingNode> GetPath((int, int) coordinates, (int, int)? origin)
        {
            Stack<PathfindingNode> path = new Stack<PathfindingNode>();
            (int, int) effectiveOrigin = origin ?? (0,0);
            PathfindingNode activeNode = FindPath(coordinates, effectiveOrigin);
           
            // No path found
            if (activeNode == null)
            {
                return null;
            }
            path.Push(activeNode);
            while (activeNode.parent != null) {
                activeNode = activeNode.parent;
                path.Push(activeNode);
            }
            return path;
        }

        public int GetMovementCost((int, int) coordinates, (int, int)? origin)
        {
            (int, int) effectiveOrigin = origin ?? (0,0);
            // Main pathfinding script
            PathfindingNode current = FindPath(coordinates, effectiveOrigin);

            if (current == null)
            {
                // No path found
                return -1;
            }

            return current.actualCost;
        }

        private static int GetDistance((int, int) origin, (int, int) destination)
        {
            return Mathf.Abs(origin.Item1 - destination.Item1) + Mathf.Abs(origin.Item2 - destination.Item2);
        }

        private bool IsValidPathOption((int, int) coordinates, ConnectionType type)
        {
            return explorationMap.GetTileStatus(coordinates) == TileStatus.EXPLORED && 
                (GameInstance.TestWalkability(type) || tileCosts.tileTypeData[type].walkableByDefault);
        }
    }
}