using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для CarsData.xaml
    /// </summary>
    public partial class CarsData : Window
    {
        public CarsData()
        {
            InitializeComponent();
            Annulate();
            BrandsCB.ItemsSource = getItems();
            BrandsCB.SelectedIndex = 0;
            ShowTable();
        }
        private String[] getItems()
        {
            List<string> AvBrands = new List<string>();
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = "select brand from automobiles";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (!AvBrands.Contains(reader[0].ToString()))
                    {
                        AvBrands.Add(reader[0].ToString());
                    }
                }
            }
            sqlConn.Close();

            AvBrands.Sort();
            String[] Items = new String[AvBrands.Count + 1];
            Items[0] = "Усі";
            for (int i = 1; i < AvBrands.Count + 1; i++)
            {
                Items[i] = AvBrands[i - 1];
            }
            return Items;
        }
        private static String Connection = @"Data Source=helgamax\mssqlservers; Initial Catalog=CarsDB;Integrated Security=True";
        SqlConnection sqlConn;
        DataTable dT1;

        int startYear;
        int startMileage;
        int endYear;
        int endMileage;
        int startPrice;
        int endPrice;
        string ID_car;
        string Brand;

        private void Annulate()
        {
            startYear = 0;
            startMileage = 0;
            endYear = 5000;
            endMileage = 10000000;
            startPrice = 0;
            endPrice = 5000000;
        }
        private void ToStartWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Close();
            mw.Show();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                startPrice = Int32.Parse(PriceStart.Text);
            }
            catch { }
            try
            {
                endPrice = Int32.Parse(PriceEnd.Text);
            }
            catch { }
            try
            {
                startMileage = Int32.Parse(MileageStart.Text);
            }
            catch { }
            try
            {
                endMileage = Int32.Parse(MileageEnd.Text);
            }
            catch { }
            try
            {
                startYear = Int32.Parse(YearStart.Text);
            }
            catch { }
            try
            {
                endYear = Int32.Parse(YearEnd.Text);
            }
            catch { }

            ShowTable();

        }
        private void ShowTable()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            string command = "";

            command = $"SELECT id_car as[ID],name as[Назва авто],year as[Рік виготовлення],price as[Ціна]," +
                $"mileage as[Пробіг] from automobiles WHERE " +
                $"(price >= {startPrice} AND price <= {endPrice}) AND (year >= {startYear} AND year<={endYear})" +
                $"AND (mileage >= {startMileage} AND mileage <= {endMileage})";

            if (BrandsCB.SelectedIndex != 0 && BrandsCB.SelectedItem != null)
            {
                Brand = BrandsCB.Text;
                command += $" AND brand = '{Brand}'";
            }

            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                var Data = new SqlDataAdapter(command, sqlConn);
                dT1 = new DataTable("cars");

                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
            }
            sqlConn.Close();
            Annulate();
        }

        private void DataGrid1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                ID_car = dT1.Rows[DataGrid1.SelectedIndex][0].ToString();
                sqlConn = new SqlConnection(Connection);
                sqlConn.Open();

                string request = $"select id_owner from automobiles where id_car = {ID_car}";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                reader.Read();
                string id_owner = reader[0].ToString();
                reader.Close();
                sqlConn.Close();

                string owner = getPersonByID(id_owner,"id_owner","owners");
                label.Content = $"Власник: {owner}";
                FindPhotoCar();

            }
        }
        private void FindPhotoCar()
        {
            try
            {  sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                var dT2 = new DataTable();
                string cars = $"  select photo_car from automobiles where id_car = {ID_car}";
                var Adapter = new SqlDataAdapter(cars, sqlConn);
                Adapter.Fill(dT2);
                MemoryStream ms = new MemoryStream((byte[])dT2.Rows[0][0]);
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = ms;
                imageSource.EndInit();
                CarPhoto.Source = imageSource;
                sqlConn.Close();
            }
            catch { }
        }
        private string getPersonByID(string ID, string idName, string TableName)
        {
            string dealer = "";
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = $"select {idName}, surname, name, patronymic from {TableName} where {idName} = {ID}";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                reader.Read();
                dealer = $"({reader[0]}) {reader[1]} {reader[2]} {reader[3]}";
                reader.Close();
                sqlConn.Close();
            }
            return dealer;
        }
    }
}
