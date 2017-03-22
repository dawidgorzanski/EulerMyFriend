using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EulerMyFriend.Model
{
    public class Node
    {
        public int ID { get; set; }
        //Początkowo null
        public Point PointOnScreen { get; set; }
        public bool StronglyConnectedComponent { get; set; }
    }
}
