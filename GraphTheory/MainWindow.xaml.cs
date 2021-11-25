using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphTheory.View;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using GraphTheory.Algorithms;

namespace GraphTheory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int[,] graph;
        public static bool showValues = true;
        private Settings settings = new Settings();
        public List<Edge> nodeList = new List<Edge>();
        
        public MainWindow()
        {
            int latestN = Settings.N;
            // DataContext = this;
            InitializeComponent();
            nodeList = GetNodeList();
            DrawLines();

            settings.AddEdgebtn.Click += delegate
            {
                Edge lastEdge = Settings.edgeList.Last();
                DrawLine(lastEdge);
            };

            settings.EditEdgebtn.Click += delegate
            {
                ClearLines();
                DrawLines();
                //Vagy csak azt a vonalat editeljük
            };

            settings.RemoveEdgebtn.Click += delegate
            {
                ClearLines();
                DrawLines();
                // Vagy csak azt a vonalat töröljük
            };

            settings.NodesCounterCB.SelectionChanged += delegate
            {
                if(latestN < Settings.N){
                    HideORShowNodes(Settings.N);
                    latestN = Settings.N;
                }
                else if(latestN > Settings.N)
                {
                    //Trace.WriteLine("MainWindow: ");
                    foreach(Edge e in Settings.edgeList)
                    {
                        //Trace.WriteLine(e.ToString());
                    }
                    HideORShowNodes(Settings.N);
                    latestN = Settings.N;
                    ClearLines();
                    DrawLines();
                }
            };
        }

        private void FillGraph()
        {
            graph = new int[Settings.N, Settings.N];
            for (int i = 0; i < Settings.N; i++)
                for (int j = 0; j < Settings.N; j++) graph[i, j] = 0;

            foreach (Edge e in Settings.edgeList)
            {
                graph[e.X, e.Y] = 1;
                graph[e.Y, e.X] = 1;
            }
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) { }
            else
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void ClearLines()
        {
            int i = 0;
            while(i < 4)
            {
                foreach (Line objLine in FindVisualChildren<Line>(GraphDisplayFrame))
                {
                    //Trace.WriteLine(objLine.Name);
                    GraphDisplayFrame.Children.Remove(objLine);
                    
                }
                i++;
            }

            
        }

        private void DrawLines()
        {
            foreach (Edge e in Settings.edgeList)
            {
                DrawLine(e);
            }
        }

        private void DrawLine(Edge edge)
        {
            Line objLine = new Line
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Black,
            };

            Edge node1 = new Edge();
            Edge node2 = new Edge();

            foreach (Edge n in nodeList)
            {
                if (edge.X == n.Value)
                {
                    objLine.X1 = n.X + 20;
                    objLine.Y1 = n.Y + 20;
                    node1 = n;
                }
                if (edge.Y == n.Value)
                {
                    objLine.X2 = n.X + 20;
                    objLine.Y2 = n.Y + 20;
                    node2 = n;
                }
            }
            // objLine.Name = "Index" + edge.X.ToString();
            //Trace.WriteLine(objLine.Name);

            Panel.SetZIndex(objLine, 1);
            _ = GraphDisplayFrame.Children.Add(objLine);

            if (showValues)
            {
                double a = Math.Abs(node1.X - node2.X);
                double b = Math.Abs(node1.Y - node2.Y);
                double angleInDegrees = Math.Atan2(a, b) * 180 / Math.PI;
                Trace.WriteLine("a= " + a + " b= " + b + " angle= " + angleInDegrees);
                double m1 = (node1.X + 20 + node2.X + 20) / 2;
                double m2 = (node1.Y + 20 + node2.Y + 20) / 2;

                TextBox txtBox = new TextBox
                {
                    Name = "txtIndex" + edge.X.ToString(),
                    Text = edge.Value.ToString(),
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    FontWeight = FontWeights.Bold,
                    RenderTransform = new RotateTransform { Angle = angleInDegrees-90 },
            };

                

                Panel.SetZIndex(txtBox, 0);
                Canvas.SetLeft(txtBox, m1);
                Canvas.SetTop(txtBox, m2);

                

                _ = GraphDisplayFrame.Children.Add(txtBox);
                
            }
        }

        public List<Edge> GetNodeList()
        {
            List<Edge> NodeList = new List<Edge>();

            foreach (var border in FindVisualChildren<Border>(GraphDisplayFrame))
            {
                List<MarkupProperty> properties = MarkupWriter.GetMarkupObjectFor(border).Properties.ToList();
                Edge node = new Edge
                {
                    X = int.Parse(properties[8].StringValue), // Canvas.left property
                    Y = int.Parse(properties[9].StringValue), // Canvas.right property
                    Value = border.Name[^1] - 48
                };
                if (NodeList.Count < Settings.N)
                {
                    NodeList.Add(node);
                }
            }
            return NodeList;
        }

        private void HideORShowNodes(int nodesCounter)
        {
            foreach (Border border in FindVisualChildren<Border>(GraphDisplayFrame))
            {
                border.Visibility = border.Name[^1] - 48 < nodesCounter ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void DFSbtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BFSbtn_Click(object sender, RoutedEventArgs e)
        {
            BFSAlgBtn.Visibility = Visibility.Visible;
        }

        private void Kruskalbtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dijkstrabtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BFSAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            FillGraph();
            BFS bfs = new BFS();
            bfs.BreadthFirstSearch();
        }
    }

    
}
