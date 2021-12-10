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
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Threading;

namespace GraphTheory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int[,] graph;
        public static bool showValues = true;
        private readonly Settings settings = new Settings();
        private DispatcherTimer timer;
        public List<Edge> nodeList = new List<Edge>();
        private readonly List<Line> lineList = new List<Line>();

        public MainWindow()
        {
            int latestN = Settings.N;
            
            InitializeComponent();
            nodeList = GetNodeList();
            DrawLines();

            settings.AddEdgebtn.Click += delegate
            {
                settings.EdgesCount = Settings.edgeList.Count();
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
                settings.EdgesCount = Settings.edgeList.Count();
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
                    HideORShowNodes(Settings.N);
                    latestN = Settings.N;
                    ClearLines();
                    DrawLines();
                }
                settings.EdgesCount = Settings.edgeList.Count();
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

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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
                foreach (TextBox txtBox in FindVisualChildren<TextBox>(GraphDisplayFrame))
                {
                    Trace.WriteLine(txtBox.Name);
                    GraphDisplayFrame.Children.Remove(txtBox);
                }
                i++;
            }

            
        }

        private void DrawLines()
        {
            lineList.Clear();
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

            objLine.Name = "line" + node1.Value.ToString() + node2.Value.ToString();

            
            Panel.SetZIndex(objLine, 1);
            _ = GraphDisplayFrame.Children.Add(objLine);

            lineList.Add(objLine);

            if (showValues)
            {
                DrawValuesToEdges(node1, node2, edge);
            }
        }

        private void DrawValuesToEdges(Edge node1, Edge node2, Edge edge)
        {
            double a = Math.Abs(node1.X - node2.X);
            double b = Math.Abs(node1.Y - node2.Y);
            double angleInDegrees = Math.Atan2(a, b) * 180 / Math.PI;
            double m1 = (node1.X + 20 + node2.X + 20) / 2;
            double m2 = (node1.Y + 20 + node2.Y + 20) / 2;

            if (node1.X <= node2.X)
            {
                if (node1.Y > node2.Y)
                {
                    angleInDegrees += 90;
                }
                else if (node1.Y <= node2.Y)
                {
                    angleInDegrees = 90 - angleInDegrees;
                }
            }
            else if (node1.X >= node2.X)
            {
                if (node1.Y > node2.Y)
                {
                    angleInDegrees = 90 - angleInDegrees;
                }
                else if (node1.Y <= node2.Y)
                {
                    angleInDegrees += 270;
                }
            }
            // Trace.WriteLine(node1.Value + "-" + node2.Value + " angle: " + angleInDegrees);
            if (angleInDegrees > 90 && angleInDegrees < 270)
            {
                angleInDegrees -= 180;
            }
            TextBox txtBox = new TextBox
            {
                Name = "txtIndex" + edge.X.ToString(),
                Text = edge.Value.ToString(),
                BorderThickness = new Thickness(0, 0, 0, 0),
                FontWeight = FontWeights.Bold,
                RenderTransform = new RotateTransform { Angle = angleInDegrees },
                IsReadOnly = true,
            };

            Panel.SetZIndex(txtBox, 0);
            Canvas.SetLeft(txtBox, m1);
            Canvas.SetTop(txtBox, m2);

            _ = GraphDisplayFrame.Children.Add(txtBox);
        }

        public List<Edge> GetNodeList()
        {
            List<Edge> NodeList = new List<Edge>();

            foreach (var border in FindVisualChildren<Border>(GraphDisplayFrame))
            {
                List<MarkupProperty> properties = MarkupWriter.GetMarkupObjectFor(border).Properties.ToList();

                if(border.Visibility == Visibility.Visible)
                {
                    Edge node = new Edge
                    {
                        X = int.Parse(properties[9].StringValue), // Canvas.left property
                        Y = int.Parse(properties[10].StringValue), // Canvas.right property
                        Value = border.Name[^1] - 48
                    };
                    if (NodeList.Count < Settings.N)
                    {
                        NodeList.Add(node);
                    }
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
            nodeList = GetNodeList();
        }


        /*
         * Addot algoritmus kirajzolása
         * Gomb lenyomást követően:
         * 1. ShowAlgorithm:
         *  - Minden node és line színét alapra állítja
         *  - létrehozza a timer-t, és el is indítja azt a Timer_Tick függvénnyel
         *  
         * 2. Timer_Tick
         *  - Timer-nél megadott időközönként ismétlődik (interval)
         *  - Végig megy az edott algoritmus által kapott int lista minden elemén(ez a lista a node-ok elérésének sorrendje)
         *  - beállítja az adott node-t pirosra, majd azt a line-t is amelyikkel eljutottunk oda
         *  - Ha végig ért a listán, akkor leállítja a Timer-t
         *  
         *  3. SetLine- és SetNodeStatus függvények a színt állítják
         */
        private int orderX, orderCount;
        private List<int> nodeOrder;
        private List<string> lineOrder;
        private void ShowAlgorithm(List<int> nodeIndices, List<string> lineIndices)
        {
            foreach(Line line in lineList)
            {
                SetLineStatus(line, 0);
            }

            for(int i=0; i<nodeList.Count; i++)
            {
                SetNodeStatus(i, 0);
            }


            nodeOrder = nodeIndices;
            lineOrder = lineIndices;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;

            orderX = 0;
            orderCount = nodeOrder.Count;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (orderX < nodeList.Count && orderX < orderCount)
            {
                SetNodeStatus(nodeOrder[orderX], 1);
                if (orderX <= lineOrder.Count && orderX > 0)
                {
                    foreach (Line line in lineList)
                    {
                        string lineCode = line.Name.Remove(0, 4);
                        if (lineCode == lineOrder[orderX-1] || Reverse(lineCode) == lineOrder[orderX-1])
                        {
                            SetLineStatus(line, 1);
                        }
                    }
                }
            }
            else
            {
                timer.Stop();
            }
            orderX++;

        }

        private void SetLineStatus(Line line, int status)
        {
            if(status == 0)
            {
                line.Stroke = Brushes.Black;
                line.Fill = Brushes.Black;
            }
            else if(status == 1)
            {
                line.Stroke = Brushes.Red;
                line.Fill = Brushes.Red;
            }
        }

        private void SetNodeStatus(int index, int status)
        {
            if (status == 0)
            {
                foreach (Border border in FindVisualChildren<Border>(GraphDisplayFrame))
                {
                    if(border.Name[^1] - 48 == index)
                    {
                        border.Background = Brushes.Wheat;
                    }
                }
            }
            else if (status == 1)
            {
                foreach (Border border in FindVisualChildren<Border>(GraphDisplayFrame))
                {
                    if (border.Name[^1] - 48 == index)
                    {
                        border.Background = Brushes.Red;
                    }
                }
            }
        }
        
        /*
         * Gombok
         */

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
            settings.Close();
        }

        private void BFSAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            FillGraph();
            Algorithms bfs = new Algorithms();
            bfs.BreadthFirstSearch();
            ShowAlgorithm(bfs.nodeIndices, bfs.lineIndices);
        }
    }


}
