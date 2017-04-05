using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerMyFriend.Model
{
    public static class EulerPath
    {
        public static Graph createEulerGraph(int nodes, out string finalEulerPath)
        {
            Random rand = new Random();
            int CounterOfConnections = 0;
            int n = nodes;
            double b = 0.5;
            //tworze losowy graf o 10 wierzchołkach i stopniu 0.5
            int[,] result = new int[n, n];
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
            Graph finalGraph=GraphCreator.CreateFromMatrix(result);
            finalEulerPath = CreateEulerPath(result, n, CounterOfConnections, finalGraph);
            return finalGraph;
        }
        public static string CreateEulerPath(int[,] result, int n, int count, Graph eulersGraph)
        {
            int CounterOfConnections = count;
            //wypisuje mój eulerowski graf
            List<int> countOfNodes = new List<int>();
            for (int i = 0; i < n; i++)
            {
                countOfNodes.Add(0);
                for (int j = 0; j < n; j++)
                {
                    if (result[i, j] == 1) countOfNodes[i]++;
                }
                countOfNodes[i] /= 2;
            }
            //stos moich podcyklów
            List<List<int>> StackOfLists = new List<List<int>>();

            //indexy w tablicy sąsiedztwa
            int x_index = 0, y_index = 0;

            //CounterOfList mówi nam ile list mamy na stosie
            int CounterOfLists = 0;

            while (true)
            {
                if (CounterOfConnections == 0) break;
                //znczy to że przeleciałem cały graf już
                StackOfLists.Add(new List<int>());
                //szukam wierzchołka które jeszcze nie jest w żadnym podgrafie
                //jeżeli nie ma jeszcze żadnego podgrafu to szukam pierwszego lepszego
                if (CounterOfLists == 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (countOfNodes[i] != 0)
                        {
                            x_index = i;
                        }
                    }
                }
                else
                {
                    foreach (int element in StackOfLists[CounterOfLists - 1])
                    {
                        if (countOfNodes[element] != 0)
                        {
                            x_index = element;
                        }
                    }
                }
                //szukam ygrekowej składowej
                for (int i = 0; i < n; i++)
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
                    for (int i = 0; i < n; i++)
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
            finalList = returnFinalList(StackOfLists, finalList, 0);
            //tworze z mojej scierzki stringa
            string EulerPath = "";
            for (int i = 0; i < finalList.Count; i++)
            {
                EulerPath = EulerPath + " " + finalList[i];
            }
            return EulerPath;
        }
        public static List<int> returnFinalList(List<List<int>> stack, List<int> final, int ListIndex)
        {
            List<int> temp = final;
            temp.Add(stack[ListIndex][0]);
            for (int i = 1; i < stack[ListIndex].Count - 1; i++)
            {
                //sprawdzam czy czasem nie muszę przeskoczyć na inny graf i go przejś
                temp.Add(stack[ListIndex][i]);
                foreach (List<int> list in stack)
                {
                    if (list[0] == stack[ListIndex][i])
                    {
                        if (list == stack[ListIndex]) continue;
                        temp = returnFinalList(stack, temp, 1 + ListIndex);
                        stack.RemoveAt(ListIndex + 1);
                        break;
                    }
                }
            }
            temp.Add(stack[ListIndex][stack[ListIndex].Count - 1]);
            return temp;
        }
    }
}
