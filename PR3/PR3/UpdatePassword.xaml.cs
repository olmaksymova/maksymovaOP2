using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;

namespace PR3
{
    /// <summary>
    /// Логика взаимодействия для UpdatePassword.xaml
    /// </summary>
    public partial class UpdatePassword : Window
    {
        public UpdatePassword()
        {
            InitializeComponent();
        }

        static SqlConnection conn;
        public static string LOGIN;
        int TryConnection = 3;
        public static void CreatePassword(string login, string pass)
        {
            var CommandUpdateLogin = new SqlCommand($"update Users set Password = '{HashPassword(pass)}' where Login = '{login}';", conn);
            CommandUpdateLogin.ExecuteNonQuery();

        }
        public static string HashPassword(string password, byte[] salt = null, bool needsOnlyHash = false)
        {
            if (salt == null || salt.Length != 16)
            {
                salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (needsOnlyHash) return hashed;
            return $"{hashed}:{Convert.ToBase64String(salt)}";
        }

        public static bool CheckHash(string hashedPasswordWithSalt, string passwordToCheck)
        {
            var passwordAndHash = hashedPasswordWithSalt.Split(':');
            if (passwordAndHash == null || passwordAndHash.Length != 2)
                return false;
            var salt = Convert.FromBase64String(passwordAndHash[1]);
            if (salt == null)
                return false;
            var hashOfpasswordToCheck = HashPassword(passwordToCheck, salt, true);
            if (String.Compare(passwordAndHash[0], hashOfpasswordToCheck) == 0)
            {
                return true;
            }
            return false;
        }
        private bool CheckPassword(string pass)
        {
            if (Regex.Match(pass, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{1,16}$").Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                string sqlConnection = @"Server = HELGAMAX\MSSQLSERVERS;" + "Database = LoginDB;" + "Integrated Security = true";
                conn = new SqlConnection(sqlConnection);
                conn.Open();

                var GetData = new SqlDataAdapter($"select Login,Password,Access,Pass_Limit from Users where Login='{Login.Text}'", conn);

                DataTable dt = new DataTable();
                GetData.Fill(dt);

                if (dt.Rows.Count != 0)
                {
                    LOGIN = Login.Text;
                    if (dt.Rows[0][1].ToString() == "")
                    {
                        if (Password.Password != "")
                        {
                            CreatePassword(Login.Text, Password.Password);
                            MessageBox.Show("Пароль оновлено!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не знайдено користувачів з таким логіном!");
                    return;
                }
                conn.Close();


                if (Login.Text == "ADMIN")
                {
                    if (CheckHash(dt.Rows[0][1].ToString(), Password.Password))
                    {
                        Administration administration = new Administration();
                        administration.Show();
                        Hide();
                    }
                    else
                    {
                        TryConnection--;
                        MessageBox.Show($"Неправильний пароль. Залишилось спроб: {TryConnection}");
                        if (TryConnection == 0)
                            App.Current.Shutdown();
                    }
                }
                else
                {
                    if (CheckHash(dt.Rows[0][1].ToString(), Password.Password))
                    {
                        if ((bool)dt.Rows[0][2] == false)
                        {
                            MessageBox.Show("Ви заблоковані. Зв'яжіться з адміном!");
                            return;
                        }

                        if ((bool)dt.Rows[0][3] == true)
                        {
                            if (!CheckPassword(Password.Password))
                            {
                                MessageBox.Show("Пароль не містить великої літери, цифри та нижнього підкреслення.\n Зв'яжіться з адміном та змініть пароль!");
                                return;
                            }
                        }

                        ChangePass up = new ChangePass();
                        up.Show();
                        Hide();
                    }
                    else
                    {
                        TryConnection--;
                        MessageBox.Show($"Неправильний пароль. Залишилось спроб: {TryConnection}");
                        if (TryConnection == 0)
                            App.Current.Shutdown();
                    }
                }
            }
            catch { MessageBox.Show("***Error***", "Неправильний формат даних!"); }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
