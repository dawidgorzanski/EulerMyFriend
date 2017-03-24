using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerMyFriend.Model
{
    public static class GraphStringValidator
    {
        //TODO - zadanie 1 cz. 1 - walidacja czy sekwencja liczb naturalnych jest ciągiem graficzny
        public static bool IsGraphString(List<int> graphicalStringGraph)
        {
            List<int> checkedList = new List<int> (graphicalStringGraph);
            var total = checkedList.Sum();
            if (total % 2 == 1) //Jesli nieparzysta suma stopni wierzcholkow - sorry, jednak nie
                return false;
            var counter = checkedList.Count;

            while (checkedList.Count > 0)
            {
                checkedList.Sort((x, y) => x.CompareTo(y));
                var current = checkedList[counter - 1];
                var indexGoingDown = 2;
                while (current > 0)
                {
                    if (counter <= current)
                        return false;
                    checkedList[counter - indexGoingDown++]--;
                    current--;
                }


                checkedList.RemoveAt((counter--)-1);
            }

            return true;
        }
        
        public static Graph CreateGraphString(List<int> graphicalStringGraph)
        {
            // Wprowadzana lista to wartosci stopni wierzcholkow
            Graph graphString = new Graph();

            int counter = graphicalStringGraph.Count;

            if (!IsGraphString(graphicalStringGraph))
                return new Graph();
            

            for (int i = 0; i < counter; i++)
            {
                graphString.Nodes.Add(new Node() { ID = i, GraphicalStringConnections = graphicalStringGraph[i]});
            }


            graphString.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
            while (graphString.Nodes.FindLastIndex(x => x.GraphicalStringConnections == 0) < graphString.Nodes.Count - 1)
            {
                var current = graphString.Nodes[counter - 1];
                var indexGoingDown = 2;
                while (current.GraphicalStringConnections > 0)
                {
                    if (graphString.Nodes[counter - indexGoingDown].GraphicalStringConnections == 0)
                    {
                        return new Graph();
                    }
                    if (counter - indexGoingDown < 0)
                        return new Graph();
                    Connection addedConnection = new Connection { Node1 = current, Node2 = graphString.Nodes[counter-indexGoingDown] };
                    graphString.AddConnection(addedConnection);

                    graphString.Nodes[counter - indexGoingDown++].GraphicalStringConnections -= 1;
                    current.GraphicalStringConnections--;
                }

                graphString.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
            }
            

            return graphString;
        }
    }
}
