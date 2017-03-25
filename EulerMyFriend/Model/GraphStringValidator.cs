using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerMyFriend.Model
{
    public static class GraphStringValidator
    {
        public static bool IsGraphString(List<int> graphicalStringGraph)
        {
            List<int> checkedList = new List<int> (graphicalStringGraph);
            var total = checkedList.Sum();
            if (total % 2 == 1) //Jesli nieparzysta suma stopni wierzcholkow - sorry, jednak nie
                return false;
            var counter = checkedList.Count;
            checkedList.Sort((x, y) => x.CompareTo(y));

            while (checkedList.Count > 0)
            {
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
                checkedList.Sort((x, y) => x.CompareTo(y));
            }

            return true;
        }
    }
}
