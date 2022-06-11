using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       public static string sqlConnection = @"Server = HELGAMAX\MSSQLSERVERS;" + "Database = LoginDB;" + "Integrated Security = true";

        public MainWindow()
        {
            InitializeComponent();
        }
        private void AdminMode_Click(object sender, RoutedEventArgs e)
        {
            UpdatePassword up = new UpdatePassword();
            up.Show();
            Hide();
        }
        private void UserMode_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            Hide();
        }
        private void UserMode_Copy_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void AboutDev_Click(object sender, RoutedEventArgs e)
        {
            DevWindow devWindow = new DevWindow();
            devWindow.Show();
        }
    }
}