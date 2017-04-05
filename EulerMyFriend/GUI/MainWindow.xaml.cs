using EulerMyFriend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EulerMyFriend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawGraph draw;
        public MainWindow()
        {
            InitializeComponent();
            InitializeColorPickers();
            draw = new DrawGraph(mainCanvas, GraphCreator.CreateFullGraph());
        }

        private void InitializeColorPickers()
        {
            colorPickerCircle.SelectedColor = Colors.Green;
            colorPickerPoints.SelectedColor = Colors.Red;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            draw.ClearAll(false);
        }

        private void colorPickerPoints_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorPoints"] = new SolidColorBrush((Color)colorPickerPoints.SelectedColor);
        }

        private void colorPickerCircle_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorCircle"] = new SolidColorBrush((Color)colorPickerCircle.SelectedColor);
        }

        private void btnOpenFromFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Pliki tekstowe | *.txt|Wszystkie pliki |*.*";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int[,] graphMatrix;
                if (SaveOpenGraph.ReadFromFile(openFileDialog.FileName, out graphMatrix))
                {
                    draw.ClearAll();

                    draw.CurrentGraph = GraphCreator.CreateFromMatrix(graphMatrix);

                    draw.NodeRadius = (int)sliderNodeRadius.Value;
                    draw.Radius = (int)sliderRadius.Value;

                    draw.DrawMainCircle();
                    draw.Draw();
                }
                else
                {
                    MessageBox.Show("Błędna zawartość pliku!", "Błąd");
                }
            }
        }

        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Macierz |*.txt|Lista |*.txt|Macierz incydencji |*.txt";

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        {
                            SaveOpenGraph.SaveToFile(saveFileDialog.FileName, draw.CurrentGraph.ToMatrixString());
                            break;
                        }
                    case 2:
                        {
                            SaveOpenGraph.SaveToFile(saveFileDialog.FileName, draw.CurrentGraph.ToListString());
                            break;
                        }
                    case 3:
                        {
                            SaveOpenGraph.SaveToFile(saveFileDialog.FileName, draw.CurrentGraph.ToIncidenceMatrixString());
                            break;
                        }
                    default:
                        {
                            SaveOpenGraph.SaveToFile(saveFileDialog.FileName, draw.CurrentGraph.ToMatrixString());
                            break;
                        }
                }
            }
        }

        private void btnDrawGraphFromNodesDegrees_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbGraphString.Text))
            {
                MessageBox.Show("Nie podano ciągu graficznego!");
                return;
            }

            try
            {
                var list = tbGraphString.Text.Trim().Split(' ').Select(Int32.Parse).ToList();

                draw.ClearAll();
                draw.CurrentGraph = GraphCreator.CreateGraphFromNodesDegrees(list);           

                if (draw.CurrentGraph.Nodes.Count == 0)
                {
                    MessageBox.Show("Z podanego ciągu graficznego nie można utworzyć grafu!");
                    return;
                }

                draw.NodeRadius = (int)sliderNodeRadius.Value;
                draw.Radius = (int)sliderRadius.Value;

                draw.DrawMainCircle();
                draw.Draw();

            }
            catch (Exception)
            {
                MessageBox.Show("Podany ciąg graficzny nie jest poprawny!");
            }
        }

        private void btnRandomizeGraph_Click(object sender, RoutedEventArgs e)
        {
            if (intUpDownNumberOfChanges.Value != null && draw.CurrentGraph.Nodes.Count > 0)
            {
                draw.ClearAll();
                if (!GraphCreator.RandomizeGraph(draw.CurrentGraph, (int)intUpDownNumberOfChanges.Value))
                {
                    MessageBox.Show("Nie można randomizować grafu!");
                }

                draw.NodeRadius = (int)sliderNodeRadius.Value;
                draw.Radius = (int)sliderRadius.Value;

                draw.DrawMainCircle();
                draw.Draw();
            }
            else
                MessageBox.Show("Niepoprawna ilość zmian, bądź graf!");
        }

        private void btnFindStronglyConnectedComponent_Click(object sender, RoutedEventArgs e)
        {
            if (draw.CurrentGraph.Nodes.Count > 0)
            {
                draw.ClearAll();
                StronglyConnectedComponent.Find(draw.CurrentGraph);

                draw.NodeRadius = (int)sliderNodeRadius.Value;
                draw.Radius = (int)sliderRadius.Value;

                draw.DrawMainCircle();
                draw.Draw();
            }          
        }

        private void btnHamiltonianGraph_Click(object sender, RoutedEventArgs e)
        {
            if (draw.CurrentGraph.Nodes.Count > 0)
            {
                if (HamiltonPath.CheckHamiltonPath(draw.CurrentGraph))
                {
                    draw.ClearAll();
                    draw.NodeRadius = (int)sliderNodeRadius.Value;
                    draw.Radius = (int)sliderRadius.Value;

                    draw.DrawMainCircle();
                    draw.Draw();
                }
                else
                {
                    MessageBox.Show("Na tym grafie nie istnieje cykl Hamiltona!");
                }               

            }
        }

        private void btnCreateKRegularGraph_Click(object sender, RoutedEventArgs e)
        {
            draw.CurrentGraph = HamiltonPath.ConstructKRegularGraph((int)intUpDownNodes.Value, (int)intUpDownEdges.Value);

            if (draw.CurrentGraph.Nodes.Count == 0)
            {
                MessageBox.Show("Z podanych wartosci nie można utworzyc grafu k-regularnego!");
                return;
            }
            draw.ClearAll();
            draw.NodeRadius = (int)sliderNodeRadius.Value;
            draw.Radius = (int)sliderRadius.Value;

            draw.DrawMainCircle();
            draw.Draw();

        }

        private void btnEulerGraph_Click(object sender, RoutedEventArgs e)
        {
            if (intUpDownNodes.Value != null)
            {
                string toWrite;
                draw.ClearAll();
                draw.CurrentGraph = EulerPath.CreateEulerGraph((int)intUpDownNodes.Value, out toWrite);

                draw.NodeRadius = (int)sliderNodeRadius.Value;
                draw.Radius = (int)sliderRadius.Value;

                draw.DrawMainCircle();
                draw.Draw();

                MessageBox.Show(toWrite, "Ścieżka Eulera");
            }
            
        }
    }
}
