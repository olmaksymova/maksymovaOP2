using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text == "" || Password.Password == "")
                MessageBox.Show("Поля логін та пароль повинні бути заповненими!");
            else
            {
                var conn = new SqlConnection(MainWindow.sqlConnection);
                conn.Open();

                var CheckSameLogin = new SqlDataAdapter($"select count (*) from users where Login ='{Login.Text}'", conn);
                DataTable dt = new DataTable();
                CheckSameLogin.Fill(dt);

                if ((int)dt.Rows[0][0] == 0)
                {
                    var CommandUpdateLogin = new SqlCommand($"insert into Users (login,password,access,pass_limit) values ('{Login.Text}','{UpdatePassword.HashPassword(Password.Password)}','1','0');", conn);
                    CommandUpdateLogin.ExecuteNonQuery();

                    if (Surname.Text != "")
                    {
                        var SurnameUpdate = new SqlCommand($"update Users set Surname = '" +
                                $"{Surname.Text}' where Login = '{Login.Text}';", conn);
                        SurnameUpdate.ExecuteNonQuery();
                    }

                    if (Name.Text != "")
                    {
                        var NameUpdate = new SqlCommand($"update Users set Name = '" +
                                $"{Name.Text}' where Login = '{Login.Text}';", conn);
                        NameUpdate.ExecuteNonQuery();
                    }

                    MessageBox.Show("Користувача додано успішно!");
                }
                else MessageBox.Show("В базіже є користувач з таким логіном!");

                conn.Close();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            Hide();
        }
    }
}
