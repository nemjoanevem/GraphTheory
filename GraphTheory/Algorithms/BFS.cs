using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GraphTheory.View;

namespace GraphTheory.Algorithms
{
    class BFS
    {                            
        List<int> nodeIndices = new List<int>(); // Ebben a listában tároljuk a meglévő csúcsok indexét
        public void BreadthFirstSearch()
        {
            Trace.WriteLine("BreadthFirstSearch");

            bool[] check = new bool[Settings.N]; // ha jártunk már az "x" csúcsban, a "check[x]" értéke TRUE
            int getX = 0;                        // melyik elemet szedjük ki legközelebb a nodeIndices-ből

            nodeIndices.Add(0);
            check[0] = true;

            while (nodeIndices.Count < Settings.N && nodeIndices.Count > getX)
            {
                int x = nodeIndices[getX];
                for (int y = 0; y < Settings.N; y++)
                {
                    if (MainWindow.graph[x, y] == 1 && check[y] == false)
                    {
                        nodeIndices.Add(y);
                        check[y] = true;
                    }
                }
                getX++;
            }

            foreach(int n in nodeIndices)
            {
                Trace.WriteLine("Nodeindices: " + n);
            }
        }
    }
}
