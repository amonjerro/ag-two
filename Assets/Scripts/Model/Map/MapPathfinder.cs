using SaveGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExplorationMap
{
    public class MapPathfinder
    {

        private class PriorityQueue
        {
            List<PathfindingNode> nodeHeap;

            public PriorityQueue()
            {
                nodeHeap = new List<PathfindingNode>();
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

        private class PathfindingNode
        {
            public (int, int) loc;
            public int estimatedCost;
            public PathfindingNode parent = null;

            public PathfindingNode(int X, int Y, int Cost)
            {
                loc = (X, Y);
                estimatedCost = Cost;
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

        public int GetMovementCost((int, int) coordinates)
        {
            int totalCost = 0;

            // Find the path
            MapTile root = explorationMap.GetTile((0,0));
            PathfindingNode node = new PathfindingNode(0, 0, GetDistance((0,0), coordinates));
            PriorityQueue queue = new PriorityQueue();
            HashSet<PathfindingNode> visited = new HashSet<PathfindingNode>();
            int candidateX;
            int candidateY;
            // Initial setup
            for (int  i = 0; i < 4; i++)
            {
                candidateX = (2-i) * (i % 2);
                candidateY = (1-i) * ((1+i) % 2);

                if ((candidateX, candidateY) == coordinates)
                {
                    // We have found our destination;
                    return tileCosts.tileTypeData[root.connections[i]].value;
                }

                // Validate that they can be enqueued at all
                if (!IsValidPathOption((candidateX, candidateY), root.connections[i]))
                {
                    continue;
                }

                queue.Enqueue(
                    new PathfindingNode(
                        candidateX, candidateY, 
                        GetDistance((candidateX, candidateY), coordinates) + tileCosts.tileTypeData[root.connections[i]].value
                        )
                    );
            }

            // Main pathfinding script
            PathfindingNode current;
            while(queue.Count > 0)
            {
                current = queue.Dequeue();
                if (current.loc == coordinates)
                {
                    // We have found our destination;
                    break;
                }
            }

            return totalCost;
        }

        private int GetDistance((int, int) origin, (int, int) destination)
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