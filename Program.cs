using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Asatr
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new int[,] {
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,9,9,9,9,0,0 },
                { 0,9,9,9,9,9,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0 },
                { 9,9,0,9,9,9,9,9,9,9 },
                { 0,0,0,0,0,0,0,0,0,0 }
            };

            var start = (X: 4, Y: 2);
            var goal = (X: 6, Y: 9);

            var theWay = AStarSearch(map, start, goal);

            if (theWay == null)
            {
                Console.WriteLine("找不到路");
                return;
            }

            map[start.Y, start.X] = 1;
            map[goal.Y, goal.X] = 5;

            var node = theWay.Parent;
            while (node.Location != start && node.Parent != null)
            {
                map[node.Location.Y, node.Location.X] = 2;
                node = node.Parent;
            }

            PrintMap(map);
        }

        static void PrintMap(int[,] map)
        {
            for (var y = 0; y < map.GetLength(0); y++)
            {
                for (var x = 0; x < map.GetLength(1); x++)
                    Console.Write($"{(map[y, x] == 2 ? "*" : map[y, x].ToString())} ");
                Console.WriteLine();
            }
        }

        static WayPoint AStarSearch(int[,] map, (int X, int Y) start, (int X, int Y) goal)
        {
            var mapSize = (Width: map.GetLength(1), Height: map.GetLength(0));
            var pointMap = new Asatr.WayPoint[mapSize.Height, mapSize.Width];

            var openedContainer = new List<Asatr.WayPoint>();
            var closedContainer = new List<Asatr.WayPoint>();

            var startNode = new Asatr.WayPoint
            {
                Location = start,
                CostFromStart = 0f,
                CostFromGoal = CalculateDistance(start, goal)
            };

            pointMap[startNode.Location.Y, startNode.Location.X] = startNode;

            openedContainer.Add(startNode);

            while (openedContainer.Count > 0)
            {
                var node = openedContainer[0];

                openedContainer.Remove(node);

                if (node.Location == goal)
                    return node;

                var t = FindNeighboringLocation(node, mapSize).ToArray();
                foreach (var neighboringLocation in FindNeighboringLocation(node, mapSize))
                {
                    if (map[neighboringLocation.Y, neighboringLocation.X] != 0)
                        continue;
                    if (node.Parent != null && node.Parent.Location == neighboringLocation)
                        continue;

                    var newCost = node.CostFromStart + CalculateDistance(node.Location, neighboringLocation);
                    var point = pointMap[neighboringLocation.Y, neighboringLocation.X]
                        ?? new Asatr.WayPoint
                        {
                            Location = neighboringLocation
                        };
                    pointMap[neighboringLocation.Y, neighboringLocation.X] = point;

                    if ((openedContainer.Contains(point) || closedContainer.Contains(point))
                        && point.CostFromStart <= newCost)
                        continue;

                    point.Parent = node;
                    point.CostFromStart = newCost;
                    point.CostFromGoal = CalculateDistance(point.Location, goal);

                    if (closedContainer.Contains(point))
                        closedContainer.Remove(point);

                    if (!openedContainer.Contains(point))
                        openedContainer.Add(point);

                    openedContainer.Sort(new WayPointComparer());
                }

                closedContainer.Add(node);
            }
            return null;
        }

        static IEnumerable<(int X, int Y)> FindNeighboringLocation(Asatr.WayPoint currentPoint, (int Width, int Height) size)
        {
            for (var x = currentPoint.Location.X - 1; x <= currentPoint.Location.X + 1; x++)
                for (var y = currentPoint.Location.Y - 1; y <= currentPoint.Location.Y + 1; y++)
                {
                    if ((x, y) == (currentPoint.Location))
                        continue;

                    if (x >= 0 && x < size.Width && y >= 0 && y < size.Height)
                        //if (x == currentPoint.Location.X || y == currentPoint.Location.Y)
                        yield return (x, y);
                }
        }

        static float CalculateDistance((int X, int Y) p1, (int X, int Y) p2)
        {
            var deltaX = p2.X - p1.X;
            var deltaY = p2.Y - p1.Y;

            var distance = (float)Math.Pow(Math.Pow(deltaX, 2f) + Math.Pow(deltaY, 2f), .5f);

            return distance;
        }
    }
}

