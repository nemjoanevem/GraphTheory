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
        readonly int N; // number of columns/rows.
        readonly int SRN; // square root of N
        readonly int K; // No. Of missing digits
        public static char[][] board = new char[9][];

        // Constructor
        public Sudoku(int N, int K)
        {
            this.N = N;
            this.K = K;

            // Compute square root of N
            double SRNd = Math.Sqrt(N);
            SRN = (int)SRNd;
            mat = new int[N, N];
        }

        // Sudoku Generator
        public void FillValues()
        {
            // Fill the diagonal of SRN x SRN matrices
            FillDiagonal();

            // Fill remaining blocks
            FillRemaining(0, SRN);

            // Remove Randomly K digits to make game
            RemoveKDigits();

            PrintSudoku();
        }

        // Fill the diagonal SRN number of SRN x SRN matrices
        void FillDiagonal()
        {

            for (int i = 0; i < N; i += SRN)

                // for diagonal box, start coordinates->i==j
                FillBox(i, i);
        }

        // Returns false if given 3 x 3 block contains num.
        bool UnUsedInBox(int rowStart, int colStart, int num)
        {
            for (int i = 0; i < SRN; i++)
                for (int j = 0; j < SRN; j++)
                    if (mat[rowStart + i, colStart + j] == num)
                        return false;

            return true;
        }

        // Fill a 3 x 3 matrix.
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

        // Random generator
        int RandomGenerator(int num)
        {
            Random rand = new Random();
            return (int)Math.Floor((double)(rand.NextDouble() * num + 1));
        }

        // Check if safe to put in cell
        bool CheckIfSafe(int i, int j, int num)
        {
            return (UnUsedInRow(i, num) &&
                    UnUsedInCol(j, num) &&
                    UnUsedInBox(i - i % SRN, j - j % SRN, num));
        }

        // check in the row for existence
        bool UnUsedInRow(int i, int num)
        {
            for (int j = 0; j < N; j++)
                if (mat[i, j] == num)
                    return false;
            return true;
        }

        // check in the row for existence
        bool UnUsedInCol(int j, int num)
        {
            for (int i = 0; i < N; i++)
                if (mat[i, j] == num)
                    return false;
            return true;
        }

        // A recursive function to Fill remaining
        // matrix
        bool FillRemaining(int i, int j)
        {
            //  System.out.println(i+" "+j);
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

        // Remove the K no. of digits to
        // complete game
        public void RemoveKDigits()
        {
            int count = K;
            while (count != 0)
            {
                int cellId = RandomGenerator(N * N) - 1;

                // extract coordinates i  and j
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

        // Print sudoku
        public void PrintSudoku()
        {
            int index = 0;
            for (int i = 0; i < N; i++)
            {
                string line = "";
                for (int j = 0; j < N; j++)
                {
                    //Console.Write(mat[i, j] + " ");
                    line += GenStringFromInt(mat[i, j]);

                }
                MainWindow.board[index++] = line.ToCharArray();
                Console.WriteLine();
            }
            Console.WriteLine();

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

        /// <summary>
        /// classical DFS algorithm to work on
        /// </summary>
        /// <param name="board"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static bool RunDepthFirstSearch(char[][] board, int row, int col)
        {
            //base case
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

        /// <summary>
        /// Given a position in the matrix, check current row and current column, and also it's small matrix 3 x 3
        /// Idea is to remove existing chars from given string "123456789" which has all chars
        /// </summary>
        /// <param name="board"></param>
        /// <param name="currentRow"></param>
        /// <param name="currentCol"></param>
        /// <returns></returns>
        private static IEnumerable<char> GetAvailableDigits(char[][] board, int currentRow, int currentCol)
        {
            var hashSet = new HashSet<char>("123456789".ToCharArray());

            for (int index = 0; index < 9; index++)
            {
                hashSet.Remove(board[currentRow][index]);
                hashSet.Remove(board[index][currentCol]);
            }

            // small 3 x 3 matrix - one of 9 3 x 3 matrixes
            var smallRow = currentRow / 3 * 3; // 0, 3, 6
            var smallCol = currentCol / 3 * 3; // 0, 3, 6

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
