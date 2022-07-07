using System.Windows;
using System.Windows.Controls;
namespace CourseWork
{
    public partial class MainWindow : Window
    {
        static public string CurrentTable = "";
        public MainWindow()
        {
            InitializeComponent();
            if (TableCB.SelectedItem != null)
                CurrentTable = TableCB.Text;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EditCustomers ed = new EditCustomers();
            ed.Show();
            Hide();
        }
        private void TableCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            CurrentTable = item.Content.ToString();
        }
        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            string NumOfRequest = RequestCB.Text.Split(' ')[1];
            switch (NumOfRequest)
            {
                case "1":
                    DealerSelect d = new DealerSelect();
                    d.Show();
                    break;
                case "2":
                    ContractsData c = new ContractsData();
                    c.Show();
                    break;
                case "3,4":
                    FindDealers h = new FindDealers();
                    h.Show();
                    break;
                case "5":
                    CarsData cars = new CarsData();
                    cars.Show();
                    break;
            }
            Close();
        }
    }
}

