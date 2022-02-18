using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp2
{

    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        static int n = Window3.N;
        static int counter = 0;
        public Window2()
        {
            InitializeComponent();
            Array.Clear(Squares,0,n*n);
            Array.Clear(Field, 0, n * n); 
            n = Window3.N;
            FirstPlayerTurn = true;
            CreateField();
           // Run.Content = Window3.N.ToString();
            counter = 0;
        }
        public void CreateField()
        {

            int x, y = (int)Turn.Height;
            BitmapImage myBitmapImage = new BitmapImage();

            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = backgr;
            myBitmapImage.EndInit();

            Height = n * 85 + 200;
            Width = n * 85 + 50;

            LayoutRoot.Height = n * 85 + 150;
            LayoutRoot.Width = n * 85 + 5;

            ToMainWindow.Width = Width;
            Run.Width = Width;
            Run.BorderThickness = new Thickness(0);
            ToMainWindow.BorderThickness =new Thickness(0);

            ToMainWindow.Margin = new Thickness(0, n * 85 + Turn.Height + Run.Height + 2, 0, 0);
            ToMainWindow.Height = Run.Height;
            Run.Margin = new Thickness(0, n * 85 + +Turn.Height + 2, 0, 0);

            for (int i = 0; i < n; i++)
            {
                x = 2;
                for (int j = 0; j < n; j++)
                {
                    Squares[i, j] = ButtonCreate(myBitmapImage.UriSource.ToString(), x, y);
                    Squares[i, j].Click += Square_click;
                    LayoutRoot.Children.Add(Squares[i, j]);
                    x += 85;
                }
                y += 85;
            }
        }
        static Uri backgr = new Uri(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\backgr.jpg");
        static Uri circle = new Uri(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\circle.png");
        static Uri cross = new Uri(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\cross.png");
        static public Image ImageCreate(string uri)
        {
            return new Image()
            {
                Source = new BitmapImage(new Uri(uri)),
                Stretch = Stretch.UniformToFill,
            };
        }
        public Button ButtonCreate(string uri, int x, int y)
        {
            return new Button()
            {
                Content = Colour,
                Height = 80,
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(x, y, 0, 0)
            };
        }
        static Button[,] Squares = new Button[15, 15];
        static bool FirstPlayerTurn = true;
        static Image Colour = ImageCreate(backgr.ToString());
        static char[,] Field = new char[15, 15];
        public bool DiagonalWin(char symb)
        {
            bool toright, toleft;
            toright = true;
            toleft = true;
            for (int i = 0; i < n; i++)
            {
                toright &= (Field[i, i] == symb);
                toleft &= Field[n - i - 1, i] == symb;
            }

            if (toright || toleft) return true;

            return false;
        }
        public bool LineWin(char symb)
        {
            bool cols, rows;
            for (int col = 0; col < n; col++)
            {
                cols = true;
                rows = true;
                for (int row = 0; row < n; row++)
                {
                    cols &= Field[col, row] == symb;
                    rows &= Field[row, col] == symb;
                }
                if (cols || rows) return true;
            }
            return false;
        }
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MainWindow MW = new();
            MW.Show();
        }
        public void TurnButtonsOff(string s)
        {
            foreach(var butt in LayoutRoot.Children)
                if (butt is Button && ((Button)butt).Name!="Run" && 
                    ((Button)butt).Name != "ToMainWindow")
                    ((Button)butt).IsEnabled = false;

            MessageBox.Show(s);
        }

        private void CreateField_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Window2 w2 = new();
            w2.Show();
        }
        public void Square_click(object sender, RoutedEventArgs e)
        {
            counter++;
            int a = 0, b = 0;
            Button ThisButton = (Button)sender;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (Squares[i, j] == ThisButton)
                    {
                        a = i;
                        b = j;
                    }
            if (ThisButton.Content == Colour)
            {
                if (!FirstPlayerTurn)
                {
                    ThisButton.Content = ImageCreate(circle.ToString());
                    Field[a, b] = 'o';
                }
                else
                {
                    ThisButton.Content = ImageCreate(cross.ToString());
                    Field[a, b] = 'x';
                }
                ThisButton.IsEnabled = false;
            }
            sender = ThisButton;
            bool resultx = false, resulto = false;
            if (FirstPlayerTurn)
                resultx = LineWin('x') || DiagonalWin('x');
            else
                resulto = LineWin('o') || DiagonalWin('o');
            if (resultx)
                TurnButtonsOff("Виграли хрестики!");
            else if (resulto)
                TurnButtonsOff("Виграли нулики!");
            else if (counter == n * n)
                MessageBox.Show("Нічия!");
            FirstPlayerTurn = !FirstPlayerTurn;
        }
    }
}
