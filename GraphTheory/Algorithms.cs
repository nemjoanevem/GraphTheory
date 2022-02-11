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

            bool[] visited = new bool[Settings.N];  // ha jártunk már az "x" csúcsban, a "visited[x]" értéke TRUE
            int getX = 0;                           // melyik elemet szedjük ki legközelebb a nodeIndices-ből

            nodeIndices.Add(getX);
            visited[getX] = true;

            while (nodeIndices.Count < Settings.N && nodeIndices.Count > getX)
            {
                int x = nodeIndices[getX];
                //Trace.WriteLine("nodeIndices[getX]: " + x);
                for (int y = 0; y < Settings.N; y++)
                {
                    //Trace.WriteLine("Y amit vizsgál: " + y);
                    if (MainWindow.graph[x, y] == 1 && visited[y] == false)
                    {
                        nodeIndices.Add(y);
                        lineIndices.Add(x.ToString() + y.ToString());
                        visited[y] = true;
                        //Trace.WriteLine("Y amit hozzá ad: " + y);
                        //Trace.WriteLine("line" + x + y);
                    }
                }
                getX++;
            }


            foreach (int n in nodeIndices)
            {
                Trace.WriteLine("Nodeindices: " + n);
            }
        }

        /*public void DepthFirstSearch()
        {
            Trace.WriteLine("DepthFirstSearch: " + Settings.N);
            bool[] visited = new bool[Settings.N];  // ha jártunk már az "x" csúcsban, a "visited[x]" értéke TRUE
            int getX = 0;                           // melyik elemet szedjük ki legközelebb a nodeIndices-ből

            nodeIndices.Add(getX);
            visited[getX] = true;

            while (nodeIndices.Count < Settings.N && nodeIndices.Count > getX)
            {
                int x = nodeIndices[getX];
                int y = 0;
                int BackX = 1;
                while (nodeIndices.Count == getX + 1)
                {
                    //Trace.WriteLine("getX: " + getX);
                    //Trace.WriteLine("BackX: " + BackX);
                    //Trace.WriteLine("X: " + x);
                    //Trace.WriteLine("Y: " + y);
                    if (y < Settings.N)
                    {
                        if (MainWindow.graph[x, y] == 1 && visited[y] == false)
                        {
                            nodeIndices.Add(y);
                            lineIndices.Add(x.ToString() + y.ToString());
                            Trace.WriteLine("Node X: " + x);
                            Trace.WriteLine("Node Y: " + y);
                            visited[y] = true;
                        }

                    }
                    else
                    {
                        y = 0;
                        if(BackX <= getX)
                        {
                            x = nodeIndices[getX - BackX];
                            BackX++;
                        }
                        else
                        {
                            BackX = 1;
                            int i = 0;
                            while (visited[i])
                            {
                                i++;
                            }
                            nodeIndices.Add(i);
                            lineIndices.Add("break");
                            visited[i] = true;
                        }
                    }
                    y++;
                }
                getX++;
            }

            foreach (int n in nodeIndices)
            {
                Trace.WriteLine("Nodeindices: " + n);
            }
        }*/

        private bool[] visited = new bool[Settings.N];
        public void DepthFirstSearch()
        {
            for (int i = 0; i < Settings.N; i++)
            {
                visited[i] = false;
            }
            for (int i = 0; i < Settings.N; i++)
            {
                if (visited[i] == false)
                {
                    bejarDFS(i);
                    lineIndices.Add("break");
                }
            }
        }

        public void bejarDFS(int v)
        {
            visited[v] = true;
            nodeIndices.Add(v);
            for (int i = 0; i < Settings.N; i++)
            {
                if (visited[i] == false && MainWindow.graph[v, i] == 1)
                {
                    lineIndices.Add(v.ToString() + i.ToString());
                    bejarDFS(i);
                }
            }

        }
    }
}
