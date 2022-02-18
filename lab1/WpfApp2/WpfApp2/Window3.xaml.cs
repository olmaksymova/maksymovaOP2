using System.Collections.Generic;
using System.IO;
using System.Windows;
namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {

        static public int N = 1;
        public Window3()
        {
            InitializeComponent();
           
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MW = new();
            Hide();
            MW.Show();
        }
 
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            N = int.Parse(Counter.Text);
            Window2 W2 = new();
            Close();
            W2.Show();
        }
    }
}
