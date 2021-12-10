using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GraphTheory
{
    class Algorithms
    {
        public readonly List<int> nodeIndices = new List<int>(); // Ebben a listában tároljuk a meglévő csúcsok indexét
        public readonly List<string> lineIndices = new List<string>();

        public void BreadthFirstSearch()
        {

            Trace.WriteLine("BreadthFirstSearch");

            bool[] check = new bool[Settings.N]; // ha jártunk már az "x" csúcsban, a "check[x]" értéke TRUE
            int getX = 0;                        // melyik elemet szedjük ki legközelebb a nodeIndices-ből
            int lastY = 0;

            nodeIndices.Add(0);
            check[0] = true;

            while (nodeIndices.Count < Settings.N && nodeIndices.Count > getX)
            {
                int x = nodeIndices[getX];
                //Trace.WriteLine("nodeIndices[getX]: " + x);
                for (int y = 0; y < Settings.N; y++)
                {
                    //Trace.WriteLine("Y amit vizsgál: " + y);
                    if (MainWindow.graph[x, y] == 1 && check[y] == false)
                    {
                        nodeIndices.Add(y);
                        lastY = y;
                        lineIndices.Add(x.ToString() + y.ToString());
                        check[y] = true;
                        //Trace.WriteLine("Y amit hozzá ad: " + y);
                        //Trace.WriteLine("line" + x + y);
                    }
                }
                getX++;
            }
            if (getX < nodeIndices.Count)
            {
                lineIndices.Add(nodeIndices[getX].ToString() + lastY.ToString());
                Trace.WriteLine("lastY = " + nodeIndices[getX - 1].ToString() + lastY.ToString());
            }


            foreach (int n in nodeIndices)
            {
                Trace.WriteLine("Nodeindices: " + n);
            }
        }
    }
}
