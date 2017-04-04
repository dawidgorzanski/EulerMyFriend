using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EulerMyFriend.Model
{
    public static class HamiltonPath //klasa ma dwie metody po to, zeby nie przekierowywac dlugiej listy argumentow przy jej wywolaniu. Algorytm sam je ustali po przekierowaniu grafu
    {
        public static bool CheckHamiltonPath(Graph myGraphToCheck)
        {
            if (myGraphToCheck.Nodes.Count > 0) // jezeli graf zawiera choc jeden wierzcholek
            {
                List<int> ActualPath = new List<int>(); //tworze liste, w ktorej bede umieszczal odwiedzone wierzcholki
                bool[] isVisited = new bool[myGraphToCheck.Nodes.Count]; // jeszcze jedna pomocnicza tablice, ktora bedzie mnie informowac, czy moge wejsc do nast. wierzcholka i dodac ja do ActualPath
                List<int> FinalPath = new List<int>(); //oraz liste, do ktorej przepisze juz sobie gotowy cykl z ActualPath (tak na wszelki wypadek)
                int v = myGraphToCheck.Nodes.ElementAt(0).ID; //zakladamy, ze zaczniemy szukanie cyklu hamiltona od pierwszego wierzcholka w myGraphToCheck
                bool isHamiltonGraph = new bool(); // flaga okreslajaca, czy algorytm znalazl cykl i czy moze przepisac go z ActualPath do FinalPath

                for (int i = 0; i < myGraphToCheck.Nodes.Count; i++)
                {
                    isVisited[i] = false; //wyzerujmy tablice odwiedzin
                }


                HamiltonAlgorithm(myGraphToCheck, v, isVisited, ActualPath, FinalPath, ref isHamiltonGraph); // wywolanie wlasciwego algorytmu
                                                                                                             // pola ActualPath i isVisited na pierwszy rzut oka wygladaja tak samo, ale odrobinke sie roznia i w niektorych momentach przechowuja rozne wartosci
                if (isHamiltonGraph) // sprwadzmy, czy nasza flaga sie zmienila
                {
                    if (FinalPath.Count > 0) // i jeszcze w ramach bezpieczenstwa, czy lista ze sciezka zostala chociaz czyms uzupelniona
                    {
                        for (int i = 0; i < (FinalPath.Count - 1); ++i) // wyszukamy krawedzie, po ktorych mozna wykonac cykl Hamiltona
                        {
                            for (int j = 0; j < myGraphToCheck.Connections.Count; j++)
                            {
                                if ((myGraphToCheck.Connections[j].Node1.ID == FinalPath.ElementAt(i) && myGraphToCheck.Connections[j].Node2.ID == FinalPath.ElementAt(i + 1)) || (myGraphToCheck.Connections[j].Node1.ID == FinalPath.ElementAt(i + 1) && myGraphToCheck.Connections[j].Node2.ID == FinalPath.ElementAt(i)))
                                {
                                    myGraphToCheck.Connections[j].LineColor = Brushes.Blue;
                                }
                            }
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        private static void HamiltonAlgorithm(Graph myGraph, int v, bool[] isVisited, List<int> ActualPath, List<int> FinalPath, ref bool isHamiltonGraph) //wlasciwy algorytm znajdowania sciezki hamiltona
        {
            ActualPath.Add(v);  //dodaje do listy odwiedzonych wierzcholkow aktualny wierzcholek v, w ktorym sie znajduje

            List<Node> neighbours = new List<Node>(); //inicjuje liste sasiadow dla wierzcholka v ( Copy&Paste od Dawida :D )
            for (int j = 0; j < myGraph.Connections.Count; j++)  // szukam dla wierzcholka v(w ktorym aktualnie jestem) sąsiadów
            {
                if (myGraph.Connections[j].Node1.ID == v)  //opcja 1: gdy wierzcholek v jest w polu Node1
                {
                    neighbours.Add(myGraph.Connections[j].Node2);
                }
                else if (myGraph.Connections[j].Node2.ID == v) //opcja 2: gdy wierzcholek v jest w polu Node2
                {
                    neighbours.Add(myGraph.Connections[j].Node1);
                }
            }
            if (ActualPath.Count == myGraph.Nodes.Count) //Tutaj sprawdzam, czy juz przypadkiem nie odwiedzilem wszystkich wierzcholkow (jezeli tak to istnieje szansa na cykl!)
            {
                for (int i = 0; i < neighbours.Count; ++i)
                {
                    if (neighbours[i].ID == ActualPath.ElementAt(0)) // Sprawdzam czy z ostatniego wierzcholka moge wrocic do poczatkowego. Jak tak, to cykl istnieje!
                    {
                        isHamiltonGraph = true;
                        break;
                    }
                }
                if (FinalPath.Count == 0 && isHamiltonGraph) //Jezeli isHamiltonGraph == true oraz jakis cykl nie zostal juz zapisany (bo moglo byc ich kilka) to zapisuje wlasnie znaleziony
                {
                    for (int i = 0; i < ActualPath.Count; ++i)
                    {
                        FinalPath.Add(ActualPath.ElementAt(i));
                    }
                }
                if (isHamiltonGraph && (FinalPath.ElementAt(0) != FinalPath.ElementAt(FinalPath.Count - 1)))
                {
                    FinalPath.Add(ActualPath.ElementAt(0)); // dodaje na koniec cyklu element poczatkowy (1->2->3 + (->1)) :D
                }
            }
            else // jednak nie odwiedzilismy jeszcze wszystkich wierzcholkow, bo dlugosc tablicy odwiedzin jest mniejsza od ilosci wierzcholkow w grafie
            {
                isVisited[v] = true;  // dodaje do tablicy odwiedzin, ze aktualnie jestem w wierzcholku v
                for (int i = 0; i < neighbours.Count; ++i) // dla kazdego sasiada v, wykonuje:
                {
                    if (isVisited[neighbours[i].ID] == false) //jezeli sposrod sasiadow wierzcholka v, wierzcholek (isVisited[neighbours[i].ID) nie byl odwiedzony:
                    {
                        HamiltonAlgorithm(myGraph, neighbours[i].ID, isVisited, ActualPath, FinalPath, ref isHamiltonGraph); //to sobie do niego wbije i sprawdze caly algorytm od nowa
                    }
                }
                isVisited[v] = false; //okazalo sie, ze wejscie do v wprowadzilo nas w slepy zaulek (brak polaczen lub wszystkie sasiadujace wierzcholki byly juz odwiedzone)
            }

            if (ActualPath.Count > 0)
            {
                ActualPath.RemoveAt(ActualPath.Count - 1); ; // wyrzucam wierzcholek v, ktory jest slepym zaulkiem
            }
        }

        public static Graph ConstructKRegularGraph(int n, int k)  // n - liczba wierzcholkow,   k - liczba krawedzi jakie maja wychodzic z jednego wierzcholka
        {
            Graph finalGraph = new Graph();
            List<int> GraphList = new List<int>();
            for (int i = 0; i < n; ++i)
            {
                GraphList.Add(k);
            }

            if (GraphStringValidator.IsGraphString(GraphList))
            {
                finalGraph = GraphCreator.CreateGraphFromNodesDegrees(GraphList);
                if (k < 3)
                {
                    GraphCreator.RandomizeGraph(finalGraph, 1);  // With special thanks to P.Augustyn
                }
                else
                {
                    GraphCreator.RandomizeGraph(finalGraph, (k / 3));   // With special thanks to P.Augustyn
                }
            }

            return finalGraph;
        }

    }
}
