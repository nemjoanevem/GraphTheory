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
using System.Text.RegularExpressions;

namespace GraphTheory
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static int[,] graph;
        private readonly bool showValues = true;
        private readonly SudSettings sudSettings = new SudSettings();
        private readonly Settings settings = new Settings();
        private DispatcherTimer timer;
        public List<Edge> nodeList = new List<Edge>();
        private readonly List<Line> lineList = new List<Line>();
        public static char[][] board = new char[9][];

        private string _CurrentLabel = "Mélységi keresés";
        public string CurrentLabel
        {
            get { return _CurrentLabel; }
            set
            {
                _CurrentLabel = value;
                OnPropertyChanged();
            }
        }

        private string _sum = "";
        public string sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged();
            }
        }


        private char[][] _BoardDisplay = board;

        public char[][] BoardDisplay
        {
            get => _BoardDisplay;
            set
            {
                _BoardDisplay = value;
                OnPropertyChanged();
            }
        }

        private int[] _Weight = new int[Settings.N];

        public int[] Weight
        {
            get => _Weight;
            set
            {
                _Weight = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            int latestN = Settings.N;
            InitializeComponent();
            DataContext = this;
            nodeList = GetNodeList();
            DrawLines();


            settings.AddEdgebtn.Click += delegate
            {
                settings.EdgesCount = Settings.edgeList.Count();
                Edge lastEdge = Settings.edgeList.Last();
                DrawLine(lastEdge);
                //Array.Clear(Weight, 0, Weight.Length);
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
                if (latestN < Settings.N){
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

            sudSettings.Savebtn.Click += delegate
            {
                UpdateBoardDisplay();
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
                    GraphDisplayFrame.Children.Remove(txtBox);
                }
                i++;
            }
            //Array.Clear(Weight, 0, Weight.Length);

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

                
                if(border.Visibility == Visibility.Visible && border.Name != "")
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
                if(border.Name != "")
                {
                    border.Visibility = border.Name[^1] - 48 < nodesCounter ? Visibility.Visible : Visibility.Hidden;
                }
            }
            nodeList = GetNodeList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        private void SetStatusToZero()
        {
            foreach (Line line in lineList)
            {
                SetLineStatus(line, 0);
            }

            for (int i = 0; i < nodeList.Count; i++)
            {
                SetNodeStatus(i, 0);
            }
        }

        /*
         * Adott algoritmus kirajzolása
         * Gomb lenyomást követően:
         * 1. ShowAlgorithm:
         *  - Minden node és line színét alapra állítja
         *  - létrehozza a timer-t, és el is indítja azt a Timer_Tick függvénnyel
         *  
         * 2. Timer_Tick
         *  - Timer-nél megadott időközönként ismétlődik (interval)
         *  - Végig megy az edott algoritmus által kapott int lista minden elemén(ez a lista a node-ok elérésének sorrendje)
         *  - beállítja az adott node-t pirosra, majd azt a line-t is amelyikkel eljutottunk oda*
         *  - Ha végig ért a listán, akkor leállítja a Timer-t
         *  
         *  *A lineOrder az egy string lista, minden eleme egy xy forma, x = kiinduló node, y = cél node, ezek a nevei az adott line-nak.
         *   Úgy tudjuk, hogy melyik line-t kell beszínezni, hogy a line-ok neve linexy vagy lineyx formában van, ez által könnyen megkereshető.
         *   A line-ok színezése egy tickkel később indul mint a node-oké, hogy követhetőbb legyen az ábra.
         *   Ha a gráf nem összefüggő, akkor ha áttérünk egy nem összefüggő szakaszra, ott bekerül egy "break" a lineOrder-be
         *   így fenn marad a követhető megjelenés.
         *  
         *  3. SetLine- és SetNodeStatus függvények a színt állítják
         */
        private int orderX, orderCount, lineOrderX;
        private List<int> nodeOrder;
        private List<string> lineOrder;
        private int algType = 0;
        private void ShowAlgorithm(List<int> nodeIndices, List<string> lineIndices, int type = 1)
        {
            SetStatusToZero();

            nodeOrder = nodeIndices;
            lineOrder = lineIndices;
            algType = type;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1.5)
            };
            timer.Tick += Timer_Tick;

            orderX = 0;
            lineOrderX = 0;
            orderCount = nodeOrder.Count;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (orderX < nodeList.Count && orderX < orderCount)
            {
                if (algType == 1)
                {
                    SetNodeStatus(nodeOrder[orderX], 1);
                    if (lineOrderX <= lineOrder.Count && orderX > 0 && orderX <= lineOrder.Count - 1)
                    {
                        if (lineOrder[orderX - 1] != "break")
                        {
                            foreach (Line line in lineList)
                            {
                                string lineCode = line.Name.Remove(0, 4);
                                if (lineCode == lineOrder[orderX - 1] || Reverse(lineCode) == lineOrder[orderX - 1])
                                {
                                    SetLineStatus(line, 1);
                                }
                            }
                            lineOrderX++;
                        }
                    }
                }
                else if(algType == 2)
                {
                    foreach (Line line in lineList)
                    {
                        string lineCode = line.Name.Remove(0, 4);
                        if (lineOrderX < lineOrder.Count && (lineCode == lineOrder[lineOrderX] || Reverse(lineCode) == lineOrder[lineOrderX]))
                        {
                            SetLineStatus(line, 1);
                            SetNodeStatus(int.Parse(lineCode.Substring(0, 1)), 1);
                            SetNodeStatus(lineCode.Last() - 48, 1);
                        }
                    }
                    lineOrderX++;
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
                    if(border.Name != "")
                    {
                        if (border.Name[^1] - 48 == index && border.Name.IndexOf("d") != 0)
                        {
                            border.Background = Brushes.Wheat;
                        }
                    }
                }
            }
            else if (status == 1)
            {
                foreach (Border border in FindVisualChildren<Border>(GraphDisplayFrame))
                {
                    if(border.Name != "")
                    {
                        if (border.Name[^1] - 48 == index)
                        {
                            border.Background = Brushes.Red;
                        }
                    }
                }
            }
        }

        private void ShowSelectedButtons(Button SelectedButton)
        {
            BFSAlgBtn.Visibility = Visibility.Hidden;
            DFSAlgBtn.Visibility = Visibility.Hidden;
            DFSSudoku.Visibility = Visibility.Hidden;
            SelectedButton.Visibility = Visibility.Visible;
        }

        private void SudokuButtons(bool show)
        {
            if (show)
            {
                Inner.Visibility = Visibility.Visible;
                DFSSudRand.Visibility = Visibility.Visible;
                DFSSudSolve.Visibility = Visibility.Visible;
                DFSSudBack.Visibility = Visibility.Visible;
                SudSettingsBtn.Visibility = Visibility.Visible;
                GraphDisplayFrame.Visibility = Visibility.Hidden;
            }
            else
            {
                Inner.Visibility = Visibility.Hidden;
                DFSSudRand.Visibility = Visibility.Hidden;
                DFSSudSolve.Visibility = Visibility.Hidden;
                DFSSudBack.Visibility = Visibility.Hidden;
                SudSettingsBtn.Visibility = Visibility.Hidden;
                GraphDisplayFrame.Visibility = Visibility.Visible;
            }
            

        }

        private void DFSButtons(bool show)
        {
            if (show)
            {
                DFSAlgBtn.Visibility = Visibility.Visible;
                DFSSudoku.Visibility = Visibility.Visible;
            }
            else
            {
                DFSAlgBtn.Visibility = Visibility.Hidden;
                DFSSudoku.Visibility = Visibility.Hidden;
            }
        }

        private void BFSButtons(bool show)
        {
            if (show)
            {
                BFSAlgBtn.Visibility = Visibility.Visible;
                //BFSPelda.Visibility = Visibility.Visible;
            }
            else
            {
                BFSAlgBtn.Visibility = Visibility.Hidden;
                //BFSPelda.Visibility = Visibility.Hidden;
            }
        }

        private void KruskalButtons(bool show)
        {
            if (show)
            {
                KruskalAlgBtn.Visibility = Visibility.Visible;
                SumLbl.Visibility = Visibility.Visible;
                SumLblTitle.Visibility = Visibility.Visible;
                //BFSPelda.Visibility = Visibility.Visible;
            }
            else
            {
                KruskalAlgBtn.Visibility = Visibility.Hidden;
                SumLbl.Visibility = Visibility.Hidden;
                SumLblTitle.Visibility = Visibility.Hidden;
                //BFSPelda.Visibility = Visibility.Hidden;
            }
        }

        private void DijkstraButtons(bool show)
        {
            if (show)
            {
                HideORShowNodeWeights(show);
                DijkstraAlgBtn.Visibility = Visibility.Visible;
                Dijkstralbl.Visibility = Visibility.Visible;
                DijkstraStart.Visibility = Visibility.Visible;
            }
            else
            {
                HideORShowNodeWeights(show);
                DijkstraAlgBtn.Visibility = Visibility.Hidden;
                Dijkstralbl.Visibility = Visibility.Hidden;
                DijkstraStart.Visibility = Visibility.Hidden;
            }
        }

        private void UpdateBoardDisplay()
        {
            BoardDisplay = board;
        }

        private void HideORShowNodeWeights(bool show)
        {
            if (show)
            {
                foreach(Label label in FindVisualChildren<Label>(GraphDisplayFrame))
                {
                    if (label.Name.ToString().IndexOf("d") == 0)
                    {
                        label.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                foreach (Label label in FindVisualChildren<Label>(GraphDisplayFrame))
                {
                    if (label.Name.ToString().IndexOf("d") == 0)
                    {
                        label.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        /*  ÁLTALÁNOS GOMBOK    */

        private void DFSbtn_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            SetStatusToZero();
            SudokuButtons(false);
            BFSButtons(false);
            KruskalButtons(false);
            DFSButtons(true);
            DijkstraButtons(false);
            CurrentLabel = "Mélységi keresés";
        }

        private void BFSbtn_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            SetStatusToZero();
            SudokuButtons(false);
            DFSButtons(false);
            KruskalButtons(false);
            BFSButtons(true);
            DijkstraButtons(false);
            CurrentLabel = "Szélességi keresés";
        }

        private void Kruskalbtn_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            SetStatusToZero();
            SudokuButtons(false);
            BFSButtons(false);
            KruskalButtons(true);
            DFSButtons(false);
            DijkstraButtons(false);
            CurrentLabel = "Kruskal algoritmus";
        }

        private void Dijkstrabtn_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            SetStatusToZero();
            SudokuButtons(false);
            BFSButtons(false);
            KruskalButtons(false);
            DFSButtons(false);
            DijkstraButtons(true);
            CurrentLabel = "Dijkstra algoritmus";
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            settings.Close();
            sudSettings.Close();
        }
        /*  SUDOKU GOMBOK   */
        private void SudSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            sudSettings.FillSettingsBoard();
            sudSettings.Show();
        }

        private void DFSSudoku_Click(object sender, RoutedEventArgs e)
        {
            DFSAlgBtn.Visibility = Visibility.Hidden;
            GraphDisplayFrame.Visibility = Visibility.Hidden;

            SudokuButtons(true);

            Sudoku s = new Sudoku(9, 40, new int [0, 0]);
            s.FillValues();
            UpdateBoardDisplay();
        }

        private void DFSSudBack_Click(object sender, RoutedEventArgs e)
        {
            DFSAlgBtn.Visibility = Visibility.Visible;
            GraphDisplayFrame.Visibility = Visibility.Visible;
            SudokuButtons(false);
        }

        private void DFSSudRand_Click(object sender, RoutedEventArgs e)
        {
            Sudoku s = new Sudoku(9, 40, new int [0, 0]);
            s.FillValues();
            UpdateBoardDisplay();
        }

        private void DFSSudSolve_Click(object sender, RoutedEventArgs e)
        {
            Sudoku.Solve();
            UpdateBoardDisplay();
        }



        /*  ALGORITMUS GOMBOK   */

        private void BFSPelda_Click(object sender, RoutedEventArgs e)
        {
            /*Random random = new Random();
            int max = (Settings.N - 1) * (Settings.N / 2);
            while (Settings.edgeList.Count != max)
            {
                Edge tempP = new Edge
                {
                    X = random.Next(Settings.N),
                    Y = random.Next(Settings.N),
                };
                if (!tempP.XEqualsY() && !tempP.IsNewCoordADuplicate(Settings.edgeList))
                {
                    Settings.edgeList.Add(tempP);
                }
            }
            ClearLines();
            DrawLines();*/
        }

        private void KruskalAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            FillGraph();
            Algorithms alg = new Algorithms();
            sum = alg.KruskalAlgorithm().ToString();
            ShowAlgorithm(alg.nodeIndices, alg.lineIndices, 2);
        }

        private void DijkstraAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            FillGraph();
            Algorithms alg = new Algorithms();
            int s = convertStringToInt(DijkstraStart.Text.ToString());
            Weight = alg.DijkstraAlgorithm(s);
        }

        private int convertStringToInt(string str)
        {
            if (str.Length > 1)
            {
                str = str.Remove(1, str.Length - 1);
            }
            int i = int.Parse(str);
            Trace.WriteLine(Settings.N);
            if(i >= Settings.N)
            {
                MessageBox.Show("A megadott indexű csúcs nem létezik!\nA(z) " + (Settings.N - 1) + ". lesz a kiinduló pont.");
                i = Settings.N - 1;
            }
            return i;
        }

        private void BFSAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            FillGraph();
            Algorithms alg = new Algorithms();
            alg.BreadthFirstSearch();
            ShowAlgorithm(alg.nodeIndices, alg.lineIndices);
        }

        private void DFSAlgBtn_Click(object sender, RoutedEventArgs e)
        {
            FillGraph();
            Algorithms alg = new Algorithms();
            alg.DepthFirstSearch();
            ShowAlgorithm(alg.nodeIndices, alg.lineIndices);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

    }

}
