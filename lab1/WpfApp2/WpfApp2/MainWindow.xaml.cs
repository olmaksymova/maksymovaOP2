using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Exit.Click -= Button_Click;
            Exit.Click += Exit_Click;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            if (MyComboBox.SelectedIndex == 0)
            {
                Window1 W1 = new();
                W1.Show();
            }
            else if (MyComboBox.SelectedIndex == 1)
            {
                Window3 W3 = new();
                W3.Show();
            }
            else if (MyComboBox.SelectedIndex == 2)
            {
                Window4 W4 = new();
                W4.Show();
            }
            else
            {
                Window5 w5=new();
                w5.Show(); 
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
