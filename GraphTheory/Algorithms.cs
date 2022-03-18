using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GraphTheory
{
    class Algorithms
    {
        public readonly List<int> nodeIndices = new List<int>(); // Ebben a listában tároljuk a meglévő csúcsok indexét
        public readonly List<string> lineIndices = new List<string>();

        /*public void BreadthFirstSearch()
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
        }*/
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

        public int KruskalAlgorithm()
        {
            List<int[]> arrayList = new List<int[]>();
            arrayList = Make_set(arrayList);
            List<Edge> sortedEdgeList = SortEdges();
            int sum = 0;
            foreach(Edge e in sortedEdgeList)
            {
                if (Find_set(arrayList, e.X, e.Y))
                {
                    if (!nodeIndices.Contains(e.X))
                    {
                        nodeIndices.Add(e.X);
                    }
                    if (!nodeIndices.Contains(e.Y))
                    {
                        nodeIndices.Add(e.Y);
                    }
                    arrayList = Union(arrayList, e.X, e.Y);
                    lineIndices.Add(e.X.ToString() + e.Y.ToString());
                    sum += e.Value;
                }
            }
            return sum;
        }

        private List<Edge> SortEdges()
        {
            /*List<Edge> sortedEdgeList = new List<Edge>();
            Edge min = new Edge();
            min.Value = 10;
            for(int i = 0; i < Settings.N; i++)
            {
                foreach (Edge e in Settings.edgeList)
                {
                    if (e.Value <= min.Value)
                    {
                        min = e;
                    }
                }
                sortedEdgeList.Add(min);
            }*/
            List<Edge> sortedEdgeList = Settings.edgeList.OrderBy(x => x.Value).ToList();
            return sortedEdgeList;
        }

        private List<int[]> Make_set(List<int[]> arrayList)
        {
            for (int y = 0; y < Settings.N; y++)
            {
                int[] arr = new int[1];
                arr[0] = y;
                arrayList.Add(arr);
            }
            return arrayList;
        }

        private bool Find_set(List<int[]> arrayList, int u, int v)
        {
            foreach (int[] i in arrayList)
            {
                if (i.Contains(u) && i.Contains(v))
                {
                    return false;
                }
            }
            return true;
        }

        private List<int[]> Union(List<int[]> arrayList, int u, int v)
        {
            int firstIndex = -1;
            int secondIndex = -1;
            foreach(int[] i in arrayList)
            {
                if (i.Contains(u))
                {
                    firstIndex = arrayList.IndexOf(i);
                }
                else if(i.Contains(v)){
                    secondIndex = arrayList.IndexOf(i);
                }
            }
            if (firstIndex != -1 && secondIndex != -1)
            {
                arrayList[firstIndex] = arrayList[firstIndex].Concat(arrayList[secondIndex]).ToArray();
                arrayList[secondIndex] = new int[0];
            }
            return arrayList;
        }

        public void BreadthFirstSearch()
        {
            for (int i = 0; i < Settings.N; i++)
            {
                visited[i] = false;
            }
            for (int i = 0; i < Settings.N; i++)
            {
                if (visited[i] == false)
                {
                    BejarBFS(i);
                    lineIndices.Add("break");
                }
            }
        }

        readonly List<int> Q = new List<int>();
        private void BejarBFS(int v)
        {
            visited[v] = true;
            nodeIndices.Add(v);
            Q.Add(v);
            while (Q.Count != 0)
            {
                int x = Q[0];
                for (int y = 0; y < Settings.N; y++)
                {
                    //Trace.WriteLine("Y amit vizsgál: " + y);
                    if (MainWindow.graph[x, y] == 1 && visited[y] == false)
                    {
                        nodeIndices.Add(y);
                        lineIndices.Add(x.ToString() + y.ToString());
                        visited[y] = true;
                        Q.Add(y);
                        //Trace.WriteLine("Y amit hozzá ad: " + y);
                        //Trace.WriteLine("line" + x + y);
                    }
                }
                Q.RemoveAt(0);
            }
        }

        private readonly bool[] visited = new bool[Settings.N];
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
                    BejarDFS(i);
                    lineIndices.Add("break");
                }
            }
        }
        public void BejarDFS(int v)
        {
            visited[v] = true;
            nodeIndices.Add(v);
            for (int i = 0; i < Settings.N; i++)
            {
                if (visited[i] == false && MainWindow.graph[v, i] == 1)
                {
                    lineIndices.Add(v.ToString() + i.ToString());
                    BejarDFS(i);
                }
            }

        }
    }

    class Sudoku
    {
        readonly int[,] mat;
        readonly int N; // Sorok és oszlopok száma
        readonly int SRN; // N gyöke
        readonly int K; // Üres mezők száma
        public static char[][] board = new char[9][];

        public Sudoku(int N, int K, int[,] m)
        {
            //Trace.WriteLine(m);
            if (m == new int [0, 0] || m.Length == 0)
            {
                //Trace.WriteLine("Az m most 9 9 lett");
                mat = new int[N, N];
            }
            else
            {
                mat = m;
            }

            this.N = N;
            this.K = K;

            double SRNd = Math.Sqrt(N);
            SRN = (int)SRNd;
            
        }

        // Sudoku Generator
        public void FillValues()
        {
            FillDiagonal();
            FillRemaining(0, SRN);
            RemoveKDigits();
            PrintSudoku();
        }
        void FillDiagonal()
        {
            for (int i = 0; i < N; i += SRN)
            {
                FillBox(i, i);
            }
        }

        // False ha az adott 3x3 blokk tartalmazza az adott számot
        public bool UnUsedInBox(int rowStart, int colStart, int num, int col = -1, int row = -1)
        {
            if(col == -1 || row == -1)
            {
                for (int i = 0; i < SRN; i++)
                    for (int j = 0; j < SRN; j++)
                        if (mat[rowStart + i, colStart + j] == num)
                            return false;
            }
            else
            {
                for (int i = 0; i < SRN; i++)
                    for (int j = 0; j < SRN; j++)
                        if (mat[rowStart + i, colStart + j] == num && (rowStart + i != row || colStart + j != col))
                            return false;
            }
            

            return true;
        }

        void FillBox(int row, int col)
        {
            int num;
            for (int i = 0; i < SRN; i++)
            {
                for (int j = 0; j < SRN; j++)
                {
                    do
                    {
                        num = RandomGenerator(N);
                    }
                    while (!UnUsedInBox(row, col, num));

                    mat[row + i, col + j] = num;
                }
            }
        }

        int RandomGenerator(int num)
        {
            Random rand = new Random();
            return (int)Math.Floor((double)(rand.NextDouble() * num + 1));
        }

        bool CheckIfSafe(int i, int j, int num)
        {
            return (UnUsedInRow(i, num) &&
                    UnUsedInCol(j, num) &&
                    UnUsedInBox(i - i % SRN, j - j % SRN, num));
        }

        // False ha az adott sor tartalmazza az adott számot
        public bool UnUsedInRow(int i, int num, int col = -1)
        {
            if(col == -1)
            {
                for (int j = 0; j < N; j++)
                    if (mat[i, j] == num)
                        return false;
            }
            else
            {
                for (int j = 0; j < N; j++)
                    if (mat[i, j] == num && j != col)
                        return false;
            }
            return true;
        }

        // False ha az adott oszlop tartalmazza az adott számot
        public bool UnUsedInCol(int j, int num, int row = -1)
        {
            if(row == -1)
            {
                for (int i = 0; i < N; i++)
                    if (mat[i, j] == num)
                        return false;
            }
            else
            {
                for (int i = 0; i < N; i++)
                    if (mat[i, j] == num && i != row)
                        return false;
            }
            return true;
        }

        bool FillRemaining(int i, int j)
        {
            if (j >= N && i < N - 1)
            {
                i++;
                j = 0;
            }
            if (i >= N && j >= N)
                return true;

            if (i < SRN)
            {
                if (j < SRN)
                    j = SRN;
            }
            else if (i < N - SRN)
            {
                if (j == (int)(i / SRN) * SRN)
                    j += SRN;
            }
            else
            {
                if (j == N - SRN)
                {
                    i++;
                    j = 0;
                    if (i >= N)
                        return true;
                }
            }

            for (int num = 1; num <= N; num++)
            {
                if (CheckIfSafe(i, j, num))
                {
                    mat[i, j] = num;
                    if (FillRemaining(i, j + 1))
                        return true;

                    mat[i, j] = 0;
                }
            }
            return false;
        }

        public void RemoveKDigits()
        {
            int count = K;
            while (count != 0)
            {
                int cellId = RandomGenerator(N * N) - 1;

                int i = cellId / N;
                int j = cellId % 9;
                if (j != 0)
                    j--;

                if (mat[i, j] != 0)
                {
                    count--;
                    mat[i, j] = 0;
                }
            }
        }

        public void PrintSudoku()
        {
            int index = 0;
            for (int i = 0; i < N; i++)
            {
                string line = "";
                for (int j = 0; j < N; j++)
                {
                    line += GenStringFromInt(mat[i, j]);
                }
                MainWindow.board[index++] = line.ToCharArray();
            }
        }

        public string GenStringFromInt(int i)
        {
            return i switch
            {
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                5 => "5",
                6 => "6",
                7 => "7",
                8 => "8",
                9 => "9",
                0 => ".",
                _ => ".",
            };
        }

        public static void Solve()
        {
            SolveSudoku(MainWindow.board);
        }
        public static void SolveSudoku(char[][] board)
        {
            RunDepthFirstSearch(board, 0, 0);
        }

        private static bool RunDepthFirstSearch(char[][] board, int row, int col)
        {
            if (row > 8)
            {
                return true;
            }

            var isLastColumn = col == 8;
            int nextRow = isLastColumn ? (row + 1) : row;
            int nextCol = isLastColumn ? 0 : (col + 1);

            var current = board[row][col];
            bool isNumber = (current - '0') >= 0 && (current - '0') <= 9;

            if (isNumber)
            {
                return RunDepthFirstSearch(board, nextRow, nextCol);
            }

            foreach (var digit in GetAvailableDigits(board, row, col))
            {
                board[row][col] = digit;

                if (RunDepthFirstSearch(board, nextRow, nextCol))
                {
                    MainWindow.board = board;
                    return true;
                }

                board[row][col] = '.';
            }

            return false;
        }


        // Megnézi az adott sort és oszlopot illetve a 3x3-as mátrixot, hogy milyen elemek vannak benne és ezeket kiszedi a "123456789" stringből

        private static IEnumerable<char> GetAvailableDigits(char[][] board, int currentRow, int currentCol)
        {
            var hashSet = new HashSet<char>("123456789".ToCharArray());

            for (int index = 0; index < 9; index++)
            {
                hashSet.Remove(board[currentRow][index]);
                hashSet.Remove(board[index][currentCol]);
            }
            var smallRow = currentRow / 3 * 3; 
            var smallCol = currentCol / 3 * 3;

            for (int row = smallRow; row < smallRow + 3; row++)
            {
                for (int col = smallCol; col < smallCol + 3; col++)
                {
                    hashSet.Remove(board[row][col]);
                }
            }

            return hashSet;
        }
    
    }
}
