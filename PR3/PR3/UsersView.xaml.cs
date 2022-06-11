using System.Data;
using System.Data.SqlClient;
using System.Windows;
namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для UsersView.xaml
    /// </summary>
    public partial class UsersView : Window
    {
        public UsersView()
        {
            InitializeComponent();
            UpdateDataTable();
            index = 0;
            ShowUser();
        }

        int index, LenTable;
        DataTable dT;
        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (index > 0)
            {
                index--;
                ShowUser();
            }
        }
        private void ShowUser()
        {
            if (dT.Rows[index][0].ToString() != "")
                UserNameSelected.Content = "Ім'я: " + dT.Rows[index][0].ToString();
            else
                UserNameSelected.Content = "Ім'я відсутнє";
            if (dT.Rows[index][0].ToString() != "")
                UserSurnameSelected.Content = "Прізвіще: " + dT.Rows[index][1].ToString();
            else
                UserSurnameSelected.Content = "Прізвище відсутнє";
            UserLoginSelected.Content = dT.Rows[index][2].ToString();
            if ((bool)dT.Rows[index][3] == true)
                UserStatusSelected.IsChecked = true;
            else
                UserStatusSelected.IsChecked = false;
            if ((bool)dT.Rows[index][4] == true)
                UserRestrictionSelected.IsChecked = true;
            else
                UserRestrictionSelected.IsChecked = false;
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (index < LenTable - 1)
            {
                index++;
                ShowUser();
            }
        }
        SqlConnection conn;
        void UpdateDataTable()
        {
            string sqlConnection = @"Server = HELGAMAX\MSSQLSERVERS;" + "Database = LoginDB;" + "Integrated Security = true";

            conn = new SqlConnection(sqlConnection);
            conn.Open();

            if (conn.State == System.Data.ConnectionState.Open)
            {
                var Data = new SqlDataAdapter("SELECT Name, Surname, Login, Access,Pass_Limit  FROM users", conn);

                dT = new DataTable("Користувачі системи");
                Data.Fill(dT);
                dataGrid.ItemsSource = dT.DefaultView;
                LenTable = dT.Rows.Count;
            }

            conn.Close();
        }

        private void UserStatusSelected_Checked(object sender, RoutedEventArgs e)
        {
            conn.Open();
            var CommandUpdateStatus = new SqlCommand($"update Users set Access = '" +
                       $"{UserStatusSelected.IsChecked}' where Login = '{UserLoginSelected.Content}';", conn);
            CommandUpdateStatus.ExecuteNonQuery();
            conn.Close();
            UpdateDataTable();
        }

        private void UserRestrictionSelected_Checked(object sender, RoutedEventArgs e)
        {
            conn.Open();
            var CommandUpdateStatus = new SqlCommand($"update Users set Pass_Limit = '" +
                       $"{UserRestrictionSelected.IsChecked}' where Login = '{UserLoginSelected.Content}';", conn);
            CommandUpdateStatus.ExecuteNonQuery();
            conn.Close();
            UpdateDataTable();
        }

        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Administration mainWindow = new Administration();
            mainWindow.Show();
            Hide();
        }
    }
}
