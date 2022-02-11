using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphTheory
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window, INotifyPropertyChanged
    {
        public static int N = 9;
        public static List<Edge> edgeList = CreateRandomEdges();
        private Edge selectedEdge;

        private int _EdgesCount = 9;
        public int EdgesCount
        {
            get { return _EdgesCount; }
            set
            {
                _EdgesCount = value;
                OnPropertyChanged();
            }
        }

        public Settings()
        {
            InitializeComponent();
            DataContext = this;
            ListView.ItemsSource = edgeList;
        }

        public static List<Edge> CreateRandomEdges()
        {
            List<Edge> list = new List<Edge>();
            int counter = 0;

            while (list.Count < N)
            {
                var random = new Random();
                Edge tempP = new Edge
                {

                    X = counter,
                    Y = random.Next(N),
                    Value = random.Next(1, N)
                };
                if (!tempP.XEqualsY() && !tempP.IsNewCoordADuplicate(list))
                {
                    list.Add(tempP);
                    counter++;
                }
                if (tempP.X == N - 1 && tempP.Y != 0 && tempP.IsNewCoordADuplicate(list))
                {
                    counter = 0;
                }
            }
            return list;
        }

        public void AddEdgebtn_Click(object sender, RoutedEventArgs e)
        {
            // //Trace.WriteLine(ListView.Items.Count);
            if (ListView.Items.Count < (int.Parse(NodesCounterCB.Text) - 1) * int.Parse(NodesCounterCB.Text) / 2) //(n-1)*n/2
            {
                bool foundEdge = false;
                Random random = new Random();

                while (!foundEdge)
                {
                    Edge tempP = new Edge
                    {
                        X = random.Next(int.Parse(NodesCounterCB.Text)),
                        Y = random.Next(int.Parse(NodesCounterCB.Text)),
                    };
                    if (!tempP.XEqualsY() && !tempP.IsNewCoordADuplicate(edgeList))
                    {
                        edgeList.Add(tempP);
                        foundEdge = true;
                    }
                }

                ListView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Elérte a maximális út számot!");
            }
        }

        private void EditEdgebtn_Click(object sender, RoutedEventArgs e)
        {
            //Trace.WriteLine(selectedEdge);

            if (int.TryParse(TxtBoxX.Text, out _) && int.TryParse(TxtBoxY.Text, out _))
            {
                if (int.Parse(TxtBoxX.Text) <= int.Parse(NodesCounterCB.Text) && int.Parse(TxtBoxY.Text) <= int.Parse(NodesCounterCB.Text))
                {
                    Edge tempP = new Edge
                    {
                        X = int.Parse(TxtBoxX.Text),
                        Y = int.Parse(TxtBoxY.Text),
                        Value = int.Parse(TxtBoxValue.Text)
                    };
                    if (tempP.X == selectedEdge.X && tempP.Y == selectedEdge.Y) // Ha csak a "value" változik
                    {
                        edgeList.Find(p => p.X == tempP.X && p.Y == tempP.Y).Value = tempP.Value;
                    }
                    else if (!tempP.XEqualsY() && !tempP.IsNewCoordADuplicate(edgeList)) // Ha "X" vagy "Y" (is) változik
                    {
                        int index = edgeList.FindIndex(p => p.X == selectedEdge.X && p.Y == selectedEdge.Y);
                        edgeList[index] = tempP;

                    }
                    else
                    {
                        MessageBox.Show("Ugyan az az x és y érték, vagy szerepel már ilyen út a gráfban.");
                    }
                    ListView.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Nem jó a csúcs érték. (nagyobb mint a max)");
                }
            }
            else
            {
                MessageBox.Show("Nem jó a csúcs érték. (nem szám)");
            }
        }

        private void RemoveEdgebtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEdge != null)
            {
                int index = edgeList.FindIndex(p => p.X == selectedEdge.X && p.Y == selectedEdge.Y);
                edgeList.RemoveAt(index);
                ListView.Items.Refresh();
            }
        }


        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEdge = (Edge)ListView.SelectedItem;
            if(selectedEdge != null)
            {
                TxtBoxValue.Text = selectedEdge.Value.ToString();
                TxtBoxX.Text = selectedEdge.X.ToString();
                TxtBoxY.Text = selectedEdge.Y.ToString();
            }
        }

        private void NodesCounterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string NodesCounterCBText = (e.AddedItems[0] as ComboBoxItem).Content as string;
           
            if (int.Parse(NodesCounterCBText) < N)
            {
                N = int.Parse(NodesCounterCBText);
                List<Edge> newEdgeList = CreateRandomEdges();
                //Trace.WriteLine("Settings: ");
                edgeList = newEdgeList;
            }
            else if (int.Parse(NodesCounterCBText) > N)
            {
                N = int.Parse(NodesCounterCBText);
            }
            if(ListView != null)
            {
                ListView.ItemsSource = edgeList;
                ListView.Items.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
