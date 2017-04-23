using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerMyFriend.Model
{
    public static class GraphCreator
    {
        //Przydało mi się #R
        public static Graph CreateFromMatrix(int[,] MatrixInt)
        {
            int Dimension = MatrixInt.GetLength(0);
            Graph fromMatrix = new Graph();

            for (int i = 0; i < Dimension; i++)
                fromMatrix.Nodes.Add(new Node() { ID = i });
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = i + 1; j < Dimension; j++)
                {
                    if (MatrixInt[i, j] == 1)
                    {
                        fromMatrix.Connections.Add(new Connection { Node1 = fromMatrix.Nodes[i], Node2 = fromMatrix.Nodes[j] });
                    }
                }
            }
            return fromMatrix;
        }

        //Zostawiłem, bo na razie gdzieś w programie jest wykorzystane, później pewnie sie nie przyda to sie usunie
        public static Graph CreateFullGraph(int Nodes = 0)
        {
            Graph fullGraph = new Graph();

            //Dodanie wierzchołków
            for (int i = 0; i < Nodes; i++)
                fullGraph.Nodes.Add(new Node() { ID = i });

            //Dodanie połączeń między wierzchołkami
            for (int i = 0; i < Nodes; i++)
            {
                for (int j = i + 1; j < Nodes; j++)
                {
                    Connection connection = new Connection();
                    connection.Node1 = fullGraph.Nodes.FirstOrDefault(x => x.ID == i);
                    connection.Node2 = fullGraph.Nodes.FirstOrDefault(x => x.ID == j);
                    fullGraph.Connections.Add(connection);
                }
            }

            return fullGraph;
        }


        //Tworzenie grafu ze stopni wierzchołków
        public static Graph CreateGraphFromNodesDegrees(List<int> graphicalStringGraph)
        {
            // Wprowadzana lista to wartosci stopni wierzcholkow
            Graph graphString = new Graph();

            int counter = graphicalStringGraph.Count;

            if (!GraphStringValidator.IsGraphString(graphicalStringGraph))
                return new Graph();


            for (int i = 0; i < counter; i++)
            {
                graphString.Nodes.Add(new Node() { ID = i, GraphicalStringConnections = graphicalStringGraph[i] });
            }


            graphString.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
            while (graphString.Nodes.FindLastIndex(x => x.GraphicalStringConnections == 0) < graphString.Nodes.Count - 1)
            {
                var current = graphString.Nodes[counter - 1];
                var indexGoingDown = 2;
                while (current.GraphicalStringConnections > 0)
                {
                    Connection addedConnection = new Connection { Node1 = current, Node2 = graphString.Nodes[counter - indexGoingDown] };
                    graphString.AddConnection(addedConnection);

                    graphString.Nodes[counter - indexGoingDown++].GraphicalStringConnections -= 1;
                    current.GraphicalStringConnections--;
                }

                graphString.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
            }

            graphString.Nodes.Sort((x, y) => x.ID.CompareTo(y.ID));

            return graphString;
        }


        //Randomizacja grafu
        public static bool RandomizeGraph(Graph oldGraph, int countChanges)
        {
            // Po randomizacji zmienia się najwieksza spójna składowa, dlatego resetuje
            oldGraph.ResetStronglyConnections();

            int connectionsCount = oldGraph.Connections.Count;
            if (connectionsCount < 2)
                return false;
            //for (int i = 0; i < oldGraph.Nodes.Count; ++i)
            //    oldGraph.Nodes[i].GraphicalStringConnections = 0;
            //for (int i = 0; i < oldGraph.Connections.Count; ++i)
            //{
            //    oldGraph.Nodes[oldGraph.Connections[i].Node1.ID].GraphicalStringConnections++;
            //    oldGraph.Nodes[oldGraph.Connections[i].Node2.ID].GraphicalStringConnections++;
            //}
            //oldGraph.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
            //if (oldGraph.Nodes.FindLastIndex(x => x.GraphicalStringConnections == 1) >= oldGraph.Nodes.Count - 2)
            //{
            //    oldGraph.Nodes.Sort((x, y) => x.ID.CompareTo(y.ID));
            //    return false;
            //}
            //oldGraph.Nodes.Sort((x, y) => x.ID.CompareTo(y.ID));
            for (int i = 0; i < connectionsCount; ++i)
            {
                if (oldGraph.Connections[i].Node1.ID > oldGraph.Connections[i].Node2.ID)
                {
                    Node temp = oldGraph.Connections[i].Node1;
                    oldGraph.Connections[i].Node1 = oldGraph.Connections[i].Node2;
                    oldGraph.Connections[i].Node2 = temp;
                }
            }

            Random rnd = new Random();
            Connection emptyConnection = oldGraph.Connections.Find(x => x.Node1.ID == 123123123);
            int secureCounter = 1000;
            while (countChanges > 0)
            {
                secureCounter--;
                if (secureCounter == 0)
                    return false;
                int id1 = rnd.Next(0, connectionsCount);
                int id2 = rnd.Next(0, connectionsCount);
                if (id1 == id2)
                    continue;
                Connection c1 = oldGraph.Connections[id1];
                Connection c2 = oldGraph.Connections[id2];
                if (c1.Node1.ID == c2.Node1.ID || c1.Node1.ID == c2.Node2.ID || c1.Node2.ID == c2.Node1.ID ||
                    c1.Node2.ID == c2.Node2.ID)
                    continue;
                Node a = c1.Node1;
                Node b = c1.Node2;
                Node c = c2.Node1;
                Node d = c2.Node2;

                if (emptyConnection == oldGraph.Connections.Find(x => x.Node1.ID == a.ID && x.Node2.ID == c.ID))
                {
                    if (b.ID > d.ID)
                    {
                        if (emptyConnection == oldGraph.Connections.Find(x => x.Node1.ID == d.ID && x.Node2.ID == b.ID))
                        {
                            c1.Node2 = c;
                            c2.Node1 = d;
                            c2.Node2 = b;
                            countChanges--;
                        }
                    }
                    else
                    {
                        if (emptyConnection == oldGraph.Connections.Find(x => x.Node1.ID == b.ID && x.Node2.ID == d.ID))
                        {
                            c1.Node2 = c;
                            c2.Node1 = b;
                            countChanges--;
                        }
                    }
                }
                else if (emptyConnection == oldGraph.Connections.Find(x => x.Node1.ID == a.ID && x.Node2.ID == d.ID))
                {
                    if (b.ID > c.ID)
                    {
                        if (emptyConnection == oldGraph.Connections.Find(x => x.Node1.ID == c.ID && x.Node2.ID == b.ID))
                        {
                            c1.Node2 = d;
                            c2.Node1 = c;
                            c2.Node2 = b;
                            countChanges--;
                        }
                    }
                    else
                    {
                        if (emptyConnection == oldGraph.Connections.Find(x => x.Node1.ID == b.ID && x.Node2.ID == c.ID))
                        {
                            c1.Node2 = d;
                            c2.Node1 = b;
                            c2.Node2 = c;
                            countChanges--;
                        }
                    }
                }
            }
            return true;
        }
    }
}
