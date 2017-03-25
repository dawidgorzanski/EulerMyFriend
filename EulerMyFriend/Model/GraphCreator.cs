using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerMyFriend.Model
{
    public static class GraphCreator
    {
        //Nie wiem czy sie przyda, ale na razie jest
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
                for (int j = i+1; j < Nodes; j++)
                {
                    Connection connection = new Connection();
                    connection.Node1 = fullGraph.Nodes.FirstOrDefault(x => x.ID == i);
                    connection.Node2 = fullGraph.Nodes.FirstOrDefault(x => x.ID == j);
                    fullGraph.Connections.Add(connection);
                }
            }          

            return fullGraph;
        }


        //TODO - zadanie 1 cz. 2 - utworzenie grafu z sekwencji liczb naturalnych
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


            return graphString;
        }


        //TODO - zadanie 2 - nie wiem czy dobrze wykminiłem w tym zadaniu - mamy utworzyć graf z innego grafu, ale zmieniając
        //połaczenia między wierzchołkami?
        public static Graph RandomizeGraph(Graph oldGraph)
        {
            int connectionsCount = oldGraph.Connections.Count;
            if (connectionsCount > 2)
                return new Graph();
            Random rnd = new Random();
            int countChanges = 3; // Pewnie do zmiany, ilosc randomizacji najpewniej zrobimy przekazujac argument do funkcji
            while (countChanges > 0)
            {
                int id1 = rnd.Next(0, connectionsCount);
                int id2 = rnd.Next(0, connectionsCount);
                if (id1 == id2)
                    continue;
                Connection c1 = oldGraph.Connections[id1];
                Connection c2 = oldGraph.Connections[id2];
                if (c1.Node1.ID == c2.Node1.ID || c1.Node1.ID == c2.Node2.ID || c1.Node2.ID == c2.Node1.ID ||
                    c1.Node2.ID == c2.Node2.ID)
                    continue;
                Node b = c1.Node2;
                Node d = c2.Node2;
                c1.Node2 = d;
                c2.Node2 = b;

                countChanges--;
            }
            return new Graph();
        }
    }
}
