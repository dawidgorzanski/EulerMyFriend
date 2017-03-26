using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EulerMyFriend.Model
{
    //TODO Prawdopodobnie trzeba będzie rozbudować klase
    public class Connection
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }
        public SolidColorBrush LineColor { get; set; }

        public Connection()
        {
            LineColor = Brushes.Black;
        }
    }
}
