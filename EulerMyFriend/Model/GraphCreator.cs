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
        public static Graph CreateGraphFromNodesDegrees(/*lista/tablica liczba naturalnych*/)
        {
            return new Graph();
        }


        //TODO - zadanie 2 - nie wiem czy dobrze wykminiłem w tym zadaniu - mamy utworzyć graf z innego grafu, ale zmieniając
        //połaczenia między wierzchołkami?
        public static Graph RandomizeGraph(Graph OldGraph)
        {
            return new Graph();
        }
    }
}
