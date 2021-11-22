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

namespace GraphTheory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Settings settings = new Settings();
        public List<Edge> nodeList = new List<Edge>();
        
        public MainWindow()
        {
            // DataContext = this;
            InitializeComponent();
            DrawLines();

            settings.AddEdgebtn.Click += delegate
            {
                Trace.WriteLine(Settings.edgeList.Count + "asdadasd");
                DrawLines();
            };
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

        public void DrawLines()
        {
            nodeList = GetNodeList();

            foreach (Edge e in Settings.edgeList)
            {
                Line objLine = new Line
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Black,
                };
                foreach (Edge n in nodeList)
                {
                    if (e.X == n.Value)
                    {
                        objLine.X1 = n.X + 20;
                        objLine.Y1 = n.Y + 20;
                    }
                    if(e.Y == n.Value)
                    {
                        objLine.X2 = n.X + 20;
                        objLine.Y2 = n.Y + 20;
                    }
                }
                _ = GraphDisplayFrame.Children.Add(objLine);
            }
        }

        public List<Edge> GetNodeList()
        {
            List<Edge> NodeList = new List<Edge>();

            foreach (var ellipse in FindVisualChildren<Ellipse>(GraphDisplayFrame))
            {
                List<MarkupProperty> properties = MarkupWriter.GetMarkupObjectFor(ellipse).Properties.ToList();
                Edge node = new Edge
                {
                    X = int.Parse(properties[7].StringValue), // Canvas.left property
                    Y = int.Parse(properties[8].StringValue), // Canvas.right property
                    Value = ellipse.Name[^1]-48
                };
                if(NodeList.Count < Settings.N)
                {
                    NodeList.Add(node);
                }
            }
            return NodeList;
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
    }

    
}
