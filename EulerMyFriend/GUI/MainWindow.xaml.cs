﻿using SimpleGraph.Model;
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
            colorPickerLines.SelectedColor = Colors.Black;
            colorPickerPoints.SelectedColor = Colors.Red;
        }


        private void btnDrawRandomGraphFromLines_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnDrawRandomGraphFromProbability_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            draw.ClearAll();
        }

        private void colorPickerPoints_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorPoints"] = new SolidColorBrush((Color)colorPickerPoints.SelectedColor);
        }

        private void colorPickerCircle_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorCircle"] = new SolidColorBrush((Color)colorPickerCircle.SelectedColor);
        }

        private void colorPickerLines_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Resources["ColorLines"] = new SolidColorBrush((Color)colorPickerLines.SelectedColor);
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
    }
}
