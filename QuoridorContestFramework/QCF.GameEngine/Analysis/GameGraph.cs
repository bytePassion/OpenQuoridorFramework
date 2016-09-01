using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using QCF.Contest.Contracts.Coordination;
using QCF.Contest.Contracts.GameElements;

namespace QCF.GameEngine.Analysis
{
    //non directed graph with edges added in both directions 
    public class GameGraph
    {
        private readonly List<FieldCoordinate> endCoordinatesForBottomPlayer = new List<FieldCoordinate>();
        private readonly List<FieldCoordinate> endCoordinatesForTopPlayer = new List<FieldCoordinate>();
        public List<Node> Graph { get; } = new List<Node>();

        public GameGraph()
        {
            InitEndCoordinates();
        }

        public GameGraph InitGraph()
        {
            return InitialGraphCreator.CreateInitialGameGraph(this);
        }

        private void InitEndCoordinates()
        {
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.A, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.B, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.C, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.D, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.E, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.F, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.G, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.H, YField.Nine));
            endCoordinatesForBottomPlayer.Add(new FieldCoordinate(XField.I, YField.Nine));

            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.A, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.B, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.C, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.D, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.E, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.F, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.G, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.H, YField.One));
            endCoordinatesForTopPlayer.Add(new FieldCoordinate(XField.I, YField.One));
        }



        public Node GetNodeForCoordinate(FieldCoordinate coordinate)
        {
            return
                Graph.Find(item =>
                    (item.Coordinate.XCoord == coordinate.XCoord) &&
                    (item.Coordinate.YCoord == coordinate.YCoord));
        }

        public void RemoveNode(FieldCoordinate coordinate)
        {
            try
            {
                Graph.Remove(GetNodeForCoordinate(coordinate));
            }
            catch (ArgumentNullException argumentNullException)
            {
                Debug.WriteLine(argumentNullException);
            }
        }

        private bool IsExitPathAvailable(FieldCoordinate startCoordinate, Player player)
        {
            var startNode = GetNodeForCoordinate(startCoordinate);
            return true;
            return TraverseGraph(startNode,
                player.PlayerType == PlayerType.BottomPlayer
                    ? endCoordinatesForBottomPlayer
                    : endCoordinatesForTopPlayer);
        }

        private bool TraverseGraph(Node startNode, IEnumerable<FieldCoordinate> targetCoords)
        {
            foreach (var targetCoord in targetCoords)
            {
                var stack = new Stack<Node>();
                stack.Push(startNode);
                startNode.Visited = true;

                while (stack.Count != 0)
                {
                    var nextNode = stack.Peek();
                    if (nextNode.Coordinate.Equals(targetCoord))
                    {
                        ClearVisitStatusForNodes();
                        return true;
                    }

                    var child = nextNode.Neighbors.FirstOrDefault(n => !n.Visited);
                    if (child == null)
                    {
                        stack.Pop();
                    }
                    else
                    {
                        child.Visited = true;
                        stack.Push(child);
                    }
                }

                ClearVisitStatusForNodes();
            }
            return false;
        }

        private void ClearVisitStatusForNodes()
        {
            foreach (var node in Graph)
                node.Visited = false;
        }

        public bool ValidateMove(FieldCoordinate sourceCoordinate, FieldCoordinate targetCoordinate, Player player)
        {
            if (!GetNodeForCoordinate(sourceCoordinate).Neighbors.Contains(GetNodeForCoordinate(targetCoordinate)))
                return false;
            if (!IsExitPathAvailable(sourceCoordinate, player))
                return false;

            return true;
        }



        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in Graph)
            {
                var elements = "";
                node.Neighbors.ForEach(value => elements += value + ",");
                builder.AppendLine($"{node.Coordinate}: {elements}");
            }

            return builder.ToString();
        }
    }
}