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
                Calculator W1 = new();
            }
            else if (MyComboBox.SelectedIndex == 1)
            {
                TTField tField = new();
            }
            else if (MyComboBox.SelectedIndex == 2)
            {
                StudentsDB W4 = new();
            }
            else
            {
                AuthorBiography w5=new();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
