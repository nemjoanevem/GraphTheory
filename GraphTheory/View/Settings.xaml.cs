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

            while (list.Count < Settings.N)
            {
                var random = new Random();
                Edge tempP = new Edge
                {

                    X = counter,
                    Y = random.Next(Settings.N),
                    Value = random.Next(1, Settings.N)
                };
                if (!tempP.XEqualsY() && !tempP.IsNewCoordADuplicate(list))
                {
                    list.Add(tempP);
                    counter++;
                }
                if (tempP.X == Settings.N - 1 && tempP.Y != 0 && tempP.IsNewCoordADuplicate(list))
                {
                    counter = 0;
                }
            }
            return list;
        }

        public void AddEdgebtn_Click(object sender, RoutedEventArgs e)
        {
            // Trace.WriteLine(ListView.Items.Count);
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
            Trace.WriteLine(selectedEdge);

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
                    if (tempP.X == selectedEdge.X && tempP.Y == selectedEdge.Y)
                    {
                        
                        
                    }
                    else if (!tempP.XEqualsY() && !tempP.IsNewCoordADuplicate(edgeList))
                    {
                       
                    }
                    else
                    {
                        MessageBox.Show("Ugyan az az x és y érték, vagy szerepel már ilyen út a gráfban.");
                    }
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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEdge = (Edge)ListView.SelectedItem;
            TxtBoxValue.Text = selectedEdge.Value.ToString();
            TxtBoxX.Text = selectedEdge.X.ToString();
            TxtBoxY.Text = selectedEdge.Y.ToString();
        }
    }
}
