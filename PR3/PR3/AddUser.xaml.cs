using System.Data;
using System.Data.SqlClient;
using System.Windows;
namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
        }

        private void AdminMode_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text != "")
            {
                SqlConnection conn;

                conn = new SqlConnection(MainWindow.sqlConnection);
                conn.Open();

                var CheckSameLogin = new SqlDataAdapter($"select count (*) from users where Login ='{Login.Text}'", conn);
                DataTable dt = new DataTable();
                CheckSameLogin.Fill(dt);

                if ((int)dt.Rows[0][0] == 0)
                {

                    var LoginCommand = new SqlCommand($"insert into users (login,access,pass_limit) values('{Login.Text}','1','0');", conn);
                    LoginCommand.ExecuteNonQuery();

                    MessageBox.Show("Користувача додано успішно!");
                }
                else MessageBox.Show("В базі вже є користувач з таким логіном!");

                conn.Close();
            }
            else MessageBox.Show("Поле логін не заповнене!");
        }

        private void ToAdminWindow_Click(object sender, RoutedEventArgs e)
        {
            Administration ad = new Administration();
            ad.Show();
            Hide();
        }
    }
}
