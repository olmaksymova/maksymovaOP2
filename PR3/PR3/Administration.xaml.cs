using System.Windows;

namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для Administration.xaml
    /// </summary>
    public partial class Administration : Window
    {
        public Administration()
        {
            InitializeComponent();
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            ChangePass c = new ChangePass();
            c.Show();
            Hide();
        }

        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        private void UsersView_Click(object sender, RoutedEventArgs e)
        {
            UsersView us = new UsersView();
            us.Show();
            Hide();
        }

        private void UserAdd_Click(object sender, RoutedEventArgs e)
        {
            AddUser ua = new AddUser();
            ua.Show();
            Hide();
        }
    }
}
