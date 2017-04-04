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
            for (int i = 0; i < oldGraph.Nodes.Count; ++i)
                oldGraph.Nodes[i].GraphicalStringConnections = 0;
            for (int i = 0; i < oldGraph.Connections.Count; ++i)
            {
                oldGraph.Nodes[oldGraph.Connections[i].Node1.ID].GraphicalStringConnections++;
                oldGraph.Nodes[oldGraph.Connections[i].Node2.ID].GraphicalStringConnections++;
            }
            oldGraph.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
            if (oldGraph.Nodes.FindLastIndex(x => x.GraphicalStringConnections == 1) >= oldGraph.Nodes.Count - 2)
            {
                oldGraph.Nodes.Sort((x, y) => x.ID.CompareTo(y.ID));
                return false;
            }
            oldGraph.Nodes.Sort((x, y) => x.ID.CompareTo(y.ID));


            Random rnd = new Random();
            Connection emptyConnection = oldGraph.Connections.Find(x => x.Node1.ID == 123123123);
            int secureCounter = 1000;
            while (countChanges > 0 && secureCounter > 0)
            {
                secureCounter--;
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
                Connection check1 = oldGraph.Connections.Find(x => (x.Node1.ID == c1.Node1.ID && x.Node2.ID == d.ID));
                if (check1 != emptyConnection)
                    continue;
                check1 = oldGraph.Connections.Find(x => (x.Node1.ID == c2.Node1.ID && x.Node2.ID == b.ID));
                if (check1 != emptyConnection)
                    continue;
                c1.Node2 = d;
                c2.Node2 = b;
                if (secureCounter == 0)
                    return false;

                countChanges--;
            }
            return true;
        }
       public static Graph createEulerGraph()
        {
            Random rand = new Random();
            int CounterOfConnections = 0;
            int n = 30;
            double b = 0.5;
            //tworze losowy graf o 10 wierzchołkach i stopniu 0.5
            int[,] result = new int[n,n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    result[i, j] = 0;
            for (int i = 1; i < n; i++)
                for (int j = 0; j < i; j++)
                    if (rand.NextDouble() < b)
                    {
                        result[i, j] = 1;
                        result[j, i] = 1;
                        CounterOfConnections++;
                    }
            //zmieniam graf aby był eulerowski

            for (int i = 0; i < n - 1; i++)
             {
                 int deg = 0;
                 for (int j = 0; j < n; j++)
                     if (result[i, j] > 0)
                         deg++;
                 //check if degree is even
                 if (deg % 2 != 0)
                 {
                     int x = rand.Next(n - i - 1) + i + 1;
                     if (result[i, x] > 0)
                     {
                         result[i, x] = 0;
                         result[x, i] = 0;
                         CounterOfConnections--;
                     }
                     else
                     {
                         result[i, x] = 1;
                         result[x, i] = 1;
                         CounterOfConnections++;
                     }
                 }
             }
            CreateEulerPath(result, n, CounterOfConnections);
            return CreateFromMatrix(result);
        }
        public static void CreateEulerPath(int [,] result, int n, int count)
        {
            int CounterOfConnections = count;
            //wypisuje mój eulerowski graf
            List<int> countOfNodes = new List<int>();
            for (int i = 0; i < n; i++)
            {
                countOfNodes.Add(0);
                Console.WriteLine();
                for (int j = 0; j < n; j++)
                {
                    Console.Write(result[i,j] + " ");
                    if(result[i, j]==1)countOfNodes[i]++;
                }
                countOfNodes[i] /= 2;
                Console.WriteLine("\t"+countOfNodes[i]+"\n");
            }
            //stos moich podcyklów
            List<List<int>> StackOfLists = new List<List<int>>();

            //indexy w tablicy sąsiedztwa
            int x_index =0, y_index=0;

            //CounterOfList mówi nam ile list mamy na stosie
            int CounterOfLists = 0;
            
            while (true)
            {
                if (CounterOfConnections == 0) break;
                //znczy to że przeleciałem cały graf już
                StackOfLists.Add(new List<int>());
                //szukam wierzchołka które jeszcze nie jest w żadnym podgrafie
                //jeżeli nie ma jeszcze żadnego podgrafu to szukam pierwszego lepszego
                if(CounterOfLists==0)
                {
                    for (int i=0;i<n;i++)
                    {
                        if (countOfNodes[i] != 0)
                        {
                            x_index = i;
                        }
                    }
                }
                else
                {
                    foreach (int element in StackOfLists[CounterOfLists-1])
                    {
                        if (countOfNodes[element] != 0)
                        {
                            x_index = element;
                        }
                    }
                }
                //szukam ygrekowej składowej
                for(int i=0;i<n;i++)
                {
                    if (result[x_index, i] == 1) y_index = i;
                }
                //kończe szukanie tego wierzchołka
                StackOfLists[CounterOfLists].Add(x_index);
                while (true)
                {
                    bool noChange = true;
                    StackOfLists[CounterOfLists].Add(y_index);
                    countOfNodes[x_index]--;
                    result[x_index, y_index] = 0;
                    result[y_index, x_index] = 0;
                    CounterOfConnections--;
                    //dodało nam wierzchołek do cyklu i teraz sprawdza czy czasem
                    //czasem nie trzeba rzucić nowej listy na stos
                    for (int i = 0; i <n; i++)
                    {
                        if (result[y_index, i] == 1)
                        {
                            x_index = y_index;
                            y_index = i;
                            noChange = false;
                            break;
                        }
                    }
                    if (noChange)
                    {
                        break; 
                    }
                }
                CounterOfLists++;
            }
            //złoże teraz mój stos w finalną liste
            List<int> finalList = new List<int>();
            finalList = returnFinalList(StackOfLists,finalList, 0);
            //Wypisze teraz mój finalny cykl
            Console.Write("\nMoja Lista to ");
            foreach (int i in finalList) Console.Write(i + " ");
        }
        public static List<int> returnFinalList(List<List<int>> stack,List<int> final, int ListIndex)
        {
            List<int> temp = final;
            temp.Add(stack[ListIndex][0]);
            for (int i=1;i<stack[ListIndex].Count-1;i++)
            {
                //sprawdzam czy czasem nie muszę przeskoczyć na inny graf i go przejś
                temp.Add(stack[ListIndex][i]);
                foreach (List<int> list in stack)
                { 
                    if (list[0]== stack[ListIndex][i])
                    {
                        if (list == stack[ListIndex]) continue;
                        temp = returnFinalList(stack, temp, ++ListIndex);
                    }
                }
            }
            temp.Add(stack[ListIndex][stack[ListIndex].Count-1]);
            return temp;
        }
    }
}
