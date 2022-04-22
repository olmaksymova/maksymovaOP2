using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    class TTField
    {
        public TTField()
        {
            InitMe();
        }

        Window WN;
        Grid LayoutRoot;
        TextBox NumField;

        public static int N = 1;
        private void InitMe()
        {
            WN = ECreate.WNCreate(250, 130, "Про автора");
            LayoutRoot = ECreate.GridCreate(WN.Width, WN.Height);
            NumField = ECreate.CreateTBox(67, 30, 140, 40, 18, new FontFamily("Segoe UI"), LayoutRoot);

            TextBlock NumberLabel = ECreate.CreateTBlock(130,45,10, 40, 20, "Розмір поля:", new FontFamily("Century Gothic"), LayoutRoot);
            Button ToMainWindow = ECreate.ButtonCreate(30, 100, NumberLabel.Height + 30, 125, "Назад", Brushes.Salmon, LayoutRoot);
            Button OK = ECreate.ButtonCreate(30, 100, NumberLabel.Height + 30, 20, "OK", Brushes.LightGreen, LayoutRoot);

            OK.Click += OK_Click;
            ToMainWindow.Click += ToMainWindow_Click;
           
            WN.Content = LayoutRoot;
            WN.Show();
        }
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            WN.Hide();
            MainWindow MW = new();
            MW.Show();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            N = int.Parse(NumField.Text);
            TicTacToe tic = new();
            WN.Close();
        }
    }
}
