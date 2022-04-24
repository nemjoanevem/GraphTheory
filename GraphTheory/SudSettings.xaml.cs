using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SudSettings.xaml
    /// </summary>
    public partial class SudSettings : Window
    {
        // private readonly char[][] tempBoard = new char[9][];
        public SudSettings()
        {
            InitializeComponent();
        }

        private string _ErrorLabel = "";
        public string ErrorLabel
        {
            get { return _ErrorLabel; }
            set
            {
                _ErrorLabel = value;
                OnPropertyChanged();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }
                }
            }
        }



        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            int zeroCounter = 0;
            if (Check_input())
            {
                foreach (TextBox tb in FindVisualChildren<TextBox>(Inner))
                {
                    if (j == 9)
                    {
                        j = 0;
                        i++;
                    }
                    if (Correct_input(tb.Text) == ".")
                    {
                        zeroCounter++;
                    }
                    if (zeroCounter <= 64)
                    {
                        //tempBoard[i][j] = Correct_input(tb.Text).ToCharArray()[0];
                        MainWindow.board[i][j] = Correct_input(tb.Text).ToCharArray()[0];


                    }
                    else
                    {
                        ErrorLabel = "Túl kevés szám van megadva";
                    }
                    j++;
                }
            }
            else
            {
                string message = "A beírt sudoku nem megoldható";
                string title = "Hiba";
                MessageBox.Show(message, title);
            }
        }

        private bool Check_input()
        {
            int[,] mat = Fill_mat_values();
            int row = 0;
            int col = 0;
            Sudoku sud = new Sudoku(9, 40, mat);

            foreach (TextBox tb in FindVisualChildren<TextBox>(Inner))
            {
                
                if (col > 8)
                {
                    col = 0;
                    row++;
                }
                if (mat[row, col] != 0)
                {
                    if (!sud.UnUsedInRow(row, mat[row, col], col) || !sud.UnUsedInCol(col, mat[row, col], row) || !sud.UnUsedInBox(Get_row_or_col_start(row), Get_row_or_col_start(col), mat[row, col], col, row))
                    {
                        return false;
                    }
                }
                col++;
            }
            return true;
        }

        private int Get_row_or_col_start(int num)
        {
            if(num <= 2)
            {
                return 0;
            }
            else if(num > 2 && num <= 5)
            {
                return 3;
            }
            else if(num > 5 && num <= 8)
            {
                return 6;
            }
            else
            {
                return -1;
            }
        }

        private int[,] Fill_mat_values()
        {
            int[,] mat = new int [9,9];
            int row = 0;
            int col = 0;
            foreach (TextBox tb in FindVisualChildren<TextBox>(Inner))
            {
                if (col > 8)
                {
                    col = 0;
                    row++;
                }
                if (Correct_input(tb.Text) == "."){
                    mat[row, col] = 0;
                }
                else
                {
                    mat[row, col] = int.Parse(Correct_input(tb.Text));
                }
                
                col++;
            }
            return mat;
        }
        


        private string Correct_input(string str)
        {
            if(str.Length > 1)
            {
                str = str.Remove(1, str.Length - 1);
            }
           
            if(str == " " || str == "0")
            {
                str = ".";
            }
            return str;
        }

        public void FillSettingsBoard()
        {
            int i = 0;
            int j = 0;
            foreach (TextBox tb in FindVisualChildren<TextBox>(Inner))
            {
                if (j == 9)
                {
                    j = 0;
                    i++;
                }
                tb.Text = char.ToString(MainWindow.board[i][j]);
                j++;
            }
        }
        /*
        private int Get_board_first_index(string tbName)
        {
            string indices = Remove_string_from_string(tbName, "NumTB");
            return indices[0] - '0';
        }
        private int Get_board_second_index(string tbName)
        {
            string indices = Remove_string_from_string(tbName, "NumTB");
            return indices[1] - '0';
        }

        private string Remove_string_from_string(string str, string stringToRemove)
        {
            int i = str.IndexOf(stringToRemove);
            return str.Remove(i, i + stringToRemove.Length);
        }
        */

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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        void close(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
