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
            var total = graphicalStringGraph.Sum();
            if (total % 2 == 1) //Jesli nieparzysta suma stopni wierzcholkow - sorry, jednak nie
                return false;
            var counter = graphicalStringGraph.Count;

            while (graphicalStringGraph.Count > 0)
            {
                graphicalStringGraph.Sort((x, y) => x.CompareTo(y));
                var current = graphicalStringGraph[counter - 1];
                var indexGoingDown = 2;
                while (current > 0)
                {
                    if (counter <= current)
                        return false;
                    graphicalStringGraph[counter - indexGoingDown++]--;
                    current--;
                }


                graphicalStringGraph.RemoveAt((counter--)-1);
            }

            return true;
        }

        // TODO niedokonczone
        public static Graph CreateGraphString(List<int> graphicalStringGraph)
        {
            Graph graphString = new Graph();

            if (!IsGraphString(graphicalStringGraph))
                return graphString;

            var counter = graphicalStringGraph.Count;
            

            for (int i = 0; i < counter; ++i)
            {
                graphString.Nodes.Add(new Node() { ID = i, GraphicalStringConnections = graphicalStringGraph[i]});
            }


            while (false)
            {
                graphString.Nodes.Sort((x, y) => x.GraphicalStringConnections.CompareTo(y.GraphicalStringConnections));
                var current = graphString.Nodes[counter - 1];
                var indexGoingDown = 2;
                while (current.GraphicalStringConnections > 0)
                {
                    Connection addedConnection = new Connection();

                    graphicalStringGraph[counter - indexGoingDown++]--;
                    current.GraphicalStringConnections--;
                }


                graphicalStringGraph.RemoveAt((counter--) - 1);
            }
            

            return graphString;
        }
    }
}
