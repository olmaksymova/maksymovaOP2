using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {
        string sqlConnection = @"Server = HELGAMAX\MSSQLSERVERS;" + "Database = LoginDB;" + "Integrated Security = true";

        public ChangePass()
        {
            InitializeComponent();
            User.Content = $"User {UpdatePassword.LOGIN}";

            conn = new SqlConnection(sqlConnection);
            conn.Open();

            var GetData = new SqlDataAdapter($"select Surname,Name from Users where Login='{log}'", conn);
            DataTable dt = new DataTable();
            GetData.Fill(dt);

            Surname.Text = dt.Rows[0][0].ToString();
            Name.Text = dt.Rows[0][1].ToString();

            conn.Close();
        }
        SqlConnection conn;

        string log = UpdatePassword.LOGIN;
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (UpdatePassword.LOGIN == "ADMIN")
            {
                Administration mw = new Administration();
                mw.Show();
            }
            else
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            Hide();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(sqlConnection);

            conn.Open();

            var GetData = new SqlDataAdapter($"select Login,Password from Users where Login='{log}'", conn);
            DataTable dt = new DataTable();
            GetData.Fill(dt);

            if (UpdatePassword.CheckHash(dt.Rows[0][1].ToString(), OldPass.Password))
            {
                if (NewPass.Password != "")
                {
                    var CommandUpdateLogin = new SqlCommand($"update Users set Password = '" +
                        $"{UpdatePassword.HashPassword(NewPass.Password)}' where Login = '{log}';", conn);
                    CommandUpdateLogin.ExecuteNonQuery();

                    MessageBox.Show("Пароль змінено успішно!");
                }
                else
                    MessageBox.Show("Новий пароль є пустою строкою!");
            }
            else
                MessageBox.Show("Введено невірний пароль!");


            if (Surname.Text != "")
            {
                var CommandUpdateLogin = new SqlCommand($"update Users set Surname = '" +
                        $"{Surname.Text}' where Login = '{log}';", conn);
                CommandUpdateLogin.ExecuteNonQuery();
            }
            if (Name.Text != "")
            {
                var CommandUpdateLogin = new SqlCommand($"update Users set Name = '" +
                        $"{Name.Text}' where Login = '{log}';", conn);
                CommandUpdateLogin.ExecuteNonQuery();
            }

            conn.Close();
        }
    }
}
