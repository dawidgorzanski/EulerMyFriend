using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EulerMyFriend.Model
{
    //Ta klasa będzie jeszcze zmieniana - będzie przyjmować obiekt Graph i na jego podstawie rysować graf
    public class DrawGraph
    {
        private Canvas _canvas;
       
        public Graph CurrentGraph { get; set; }
        public int Radius { get; set; }
        public int NodeRadius { get; set; }

        public DrawGraph(Canvas canvas, Graph graph)
        {
            this.CurrentGraph = graph;
            this._canvas = canvas;
        }

        //rysowanie głównego koła
        public void DrawMainCircle()
        {
            Ellipse mainEllipse = new Ellipse();
            mainEllipse.SetResourceReference(Ellipse.StrokeProperty, "ColorCircle");
            mainEllipse.StrokeThickness = 1;
            mainEllipse.Height = mainEllipse.Width = 2 * Radius;

            //Ustawiane w ten sposób, gdyz punkt (0,0) elementu to lewy górny róg, a nie jego środek
            Canvas.SetLeft(mainEllipse, (_canvas.ActualWidth / 2) - Radius + (NodeRadius / 2));
            Canvas.SetTop(mainEllipse, (_canvas.ActualHeight / 2) - Radius + (NodeRadius / 2));

            _canvas.Children.Insert(0, mainEllipse);
        }

        //rysowanie punktów
        private void DrawNodes()
        {
            double a = _canvas.ActualWidth / 2;
            double b = _canvas.ActualHeight / 2;

            for (int i = 0; i < CurrentGraph.Nodes.Count; i++)
            {
                double t = 2 * Math.PI * i / CurrentGraph.Nodes.Count;
                int x = (int)Math.Round(a + Radius * Math.Cos(t));
                int y = (int)Math.Round(b + Radius * Math.Sin(t));

                CurrentGraph.Nodes[i].PointOnScreen = new Point(x, y);

                Ellipse ellipse = new Ellipse();
                ellipse.SetResourceReference(Ellipse.StrokeProperty, "ColorPoints");
                ellipse.SetResourceReference(Ellipse.FillProperty, "ColorPoints");
                ellipse.Height = NodeRadius;
                ellipse.Width = NodeRadius;
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
                _canvas.Children.Add(ellipse);

                Label label = new Label();
                label.Height = 50;
                label.Width = 50;
                label.Content = CurrentGraph.Nodes[i].ID;
                Canvas.SetLeft(label, x - 15);
                Canvas.SetTop(label, y - 15);
                _canvas.Children.Add(label);
            }
        }

        //rysowanie pełnego grafu
        public bool Draw()
        {
            if (CurrentGraph.Nodes.Count == 0)
                return false;

            //Rysowanie punktów
            DrawNodes();

            //Rysowanie linii
            foreach(Connection connection in CurrentGraph.Connections)
            {
                DrawLine(connection);
            }

            return true;
        }

        //rysowanie linii od punktu node1 do punktu node2
        private void DrawLine(Connection connection)
        {
            Line line = new Line();
            line.StrokeThickness = 1;

            line.Stroke = connection.LineColor;

            line.X1 = connection.Node1.PointOnScreen.X + NodeRadius / 2;
            line.X2 = connection.Node2.PointOnScreen.X + NodeRadius / 2;
            line.Y1 = connection.Node1.PointOnScreen.Y + NodeRadius / 2;
            line.Y2 = connection.Node2.PointOnScreen.Y + NodeRadius / 2;
            //Insert() zamiast Add(), aby linie były "pod spodem" - liczy się kolejność dodawania, im dalej na liście tym "wyżej"
            _canvas.Children.Insert(0, line);
        }

        public void ClearAll(bool OnlyView = true)
        {
            //OnlyView - czyści tylko "rysunek"
            //żeby nie było null
            if (!OnlyView)
                CurrentGraph = GraphCreator.CreateFullGraph();

            _canvas.Children.Clear();
        }
    }
}
