using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphTheory.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public static int N = 9;
        public static List<Edge> edgeList = CreateRandomEdges();
        private Edge selectedEdge;

        public Settings()
        {
            InitializeComponent();
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
            int index = edgeList.FindIndex(p => p.X == selectedEdge.X && p.Y == selectedEdge.Y);
            edgeList.RemoveAt(index);
            ListView.Items.Refresh();
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
                foreach (Edge edge in Settings.edgeList)
                {
                    //Trace.WriteLine(edge.ToString());
                }
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
    }
}
