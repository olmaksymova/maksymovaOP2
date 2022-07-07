using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для FindDealers.xaml
    /// </summary>
    public partial class FindDealers : Window
    {
        public FindDealers()
        {
            InitializeComponent();
            CitiesCB.ItemsSource = FindCities();
            CitiesCB.SelectedIndex = 0;
            SelectedCity = CitiesCB.Text;
            ShowTable("dealers", DataGrid1, "id_dealer");
            ShowTable("customers", DataGrid2, "id_customer");
            getMaxSalary();
            getRichestCustomer();
        }
        string SelectedCity = "";
        private static String Connection = @"Data Source=helgamax\mssqlservers; Initial Catalog=CarsDB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlConnection sqlConn;
        DataTable dT;
        private void ToStartWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Close();
            mw.Show();

        }
        private String[] FindCities()
        {
            List<string> AvailableCities = new List<string>();
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = "select city from customers";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (!AvailableCities.Contains(reader[0].ToString()))
                    {
                        AvailableCities.Add(reader[0].ToString());
                    }
                }

            }
            sqlConn.Close();

            AvailableCities.Sort();
            String[] Items = new String[AvailableCities.Count];
            for (int i = 0; i < AvailableCities.Count; i++)
            {
                Items[i] = AvailableCities[i];
            }
            return Items;
        }
        private void getMaxSalary()
        {
            Dictionary<int, List<int>> DealerSalary = new Dictionary<int, List<int>>();

            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = "select id_dealer, comission from contracts";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    int key = Int32.Parse(reader[0].ToString());
                    int value = Int32.Parse(reader[1].ToString());
                    if (DealerSalary.ContainsKey(key))
                    {
                        DealerSalary[key].Add(value);
                    }
                    else
                    {
                        DealerSalary.Add(key, new List<int> { value });
                    }
                }
                reader.Close();
                sqlConn.Close();
                int MaxSumIndex = -1, MaxSum = 0, MaxComission = 0, MaxComIndex = -1;
                foreach (var a in DealerSalary)
                {
                    int currentSum = a.Value.Sum();
                    if (currentSum > MaxSum)
                    {
                        MaxSumIndex = a.Key;
                        MaxSum = currentSum;
                    }
                    if (a.Value.Max() > MaxComission)
                    {
                        MaxComission = a.Value.Max();
                        MaxComIndex = a.Key;
                    }
                }
                label1.Content = $"Дилер з найбільшим прибутком: {getPersonByID($"{MaxSumIndex}", "id_dealer", "dealers")}, {MaxSum}$";
                label2.Content = $"Дилер з макс. комісійними: {getPersonByID($"{MaxComIndex}", "id_dealer", "dealers")}, {MaxComission}$";
            }
        }
        private void getRichestCustomer()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = "select id_customer, price from contracts";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                int maxID = -1, maxPrice = 0;
                while (reader.Read())
                {
                    int ID = Int32.Parse(reader[0].ToString());
                    int price = Int32.Parse(reader[1].ToString());

                    if (price > maxPrice)
                    {
                        maxPrice = price;
                        maxID = ID;
                    }
                }
                label3.Content = $"Клієнт, що купив найдорожче авто: {getPersonByID($"{maxID}", "id_customer", "customers")}, {maxPrice}$";

                reader.Close();
                sqlConn.Close();
            }
        }
        private string getPersonByID(string ID, string idName, string TableName)
        {
            string person = "";
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = $"select {idName}, surname, name, patronymic from {TableName} where {idName} = {ID}";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                reader.Read();
                person = $"({reader[0]}) {reader[1]} {reader[2]} {reader[3]}";
                reader.Close();
                sqlConn.Close();
            }
            return person;
        }
        private void ShowTable(string TableName, DataGrid d, string id)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter($" select {id} as[ID], surname as[Прізвище],name as[Ім'я],patronymic as[Пo батькові], " +
                    $"phone as[Телефон] from {TableName} where city = '{SelectedCity}'", sqlConn);
                dT = new DataTable($"{TableName}");
                Data.Fill(dT);
                d.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
        }

        private void CitiesCB_DropDownClosed(object sender, EventArgs e)
        {
            if (CitiesCB.SelectedItem != null)
            {
                SelectedCity = CitiesCB.Text;
                ShowTable("dealers", DataGrid1, "id_dealer");
                ShowTable("customers", DataGrid2, "id_customer");
            }
        }
    }
}
