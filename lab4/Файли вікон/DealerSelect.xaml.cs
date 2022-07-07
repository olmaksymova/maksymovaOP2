using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CourseWork
{
    public partial class DealerSelect : Window
    {
        private static String Connection = @"Data Source=helgamax\mssqlservers; Initial Catalog=CarsDB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlConnection sqlConn;
        DataTable dT1;
        DataTable dT;
        string ID = "1";
        static public string IDContract = "1";
        public DealerSelect()
        {
            InitializeComponent();
            DealersCB.ItemsSource = GetItems();
            DealersCB.SelectedIndex = 0;
            ShowTable();
        }
        private void CBoxItems_SelectionChanged(object sender,
      SelectionChangedEventArgs e)
        {
            try
            {
                if (DealersCB.SelectedItem == null)
                    DealersCB.SelectedIndex = 0;
                String Text = DealersCB.SelectedItem.ToString();

            }
            catch { }
            try
            {
                int index = DealersCB.SelectedIndex;
                ID = dT1.Rows[index][0].ToString();
                MemoryStream ms = new MemoryStream((byte[])dT1.Rows[index][7]);
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = ms;
                imageSource.EndInit();
                DealerPhoto.Source = imageSource;
            }
            catch
            { }
            ShowTable();
            ContractCount.Content = "Кількість договорів дилера: " + DataGrid1.Items.Count;
        }
        private void ToStartWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Close();
            mw.Show();
        }
        private void ShowTable()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                var Data = new SqlDataAdapter($" select id_contract as [ID],date_contract as[Дата підписання угоди],date_sell ," +
                    $"surname as[Прізвище покупця],name as[Ім'я],patronymic as[Пo батькові],phone as[Телефон],comission as[Сплачена комісія] from contracts" +
                    $" d inner join customers c ON d.id_customer = c.id_customer WHERE d.id_dealer = {ID}", sqlConn);

                dT = new DataTable($"dealers");

                Data.Fill(dT);
                int count = dT.Columns.Count;

                DataColumn NewCol = new DataColumn();
                NewCol.ColumnName = "IsContractValid";
                dT.Columns.Add(NewCol);

                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if ((DateTime)dT.Rows[i][2] < DateTime.Today)
                    {
                        dT.Rows[i][count] = "Так";
                    }
                    else
                    {
                        dT.Rows[i][count] = "Ні";
                    }
                }
                dT.Columns.RemoveAt(2);
                dT.Columns[dT.Columns.Count - 1].ColumnName = "Чи дійсний котракт";
                DataGrid1.ItemsSource = dT.DefaultView;
            }
            sqlConn.Close();
        }
        private String[] GetItems()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            String[] Items = { "" };
            var nums = new List<int>();

            List<int> AvailableCars = new List<int>();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter($"SELECT * FROM dealers", sqlConn);

                dT1 = new DataTable($"dealers");

                Data.Fill(dT1);
                Items = new String[dT1.Rows.Count];

                for (int i = 0; i < dT1.Rows.Count; i++)
                {
                    Items[i] = dT1.Rows[i][0].ToString() +
                    " " + dT1.Rows[i][1].ToString() + " " +
                    dT1.Rows[i][2].ToString() + " " + dT1.Rows[i][3].ToString();

                    AvailableCars.Add(Int32.Parse(dT1.Rows[i][0].ToString()));
                }
            }
            sqlConn.Close();
            return Items;
        }
        private void OpenContract_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                ShowContract s = new ShowContract();
                Close(); s.Show();
                s.ShowData(IDContract, true);
            }
        }
        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DataGrid1.SelectedItem != null)
            {
                try
                {
                    IDContract = dT.Rows[DataGrid1.SelectedIndex][0].ToString();

                    sqlConn = new SqlConnection(Connection);
                    sqlConn.Open();
                    var dT2 = new DataTable();
                    string cars = $"  select photo_car from automobiles d inner join contracts c ON c.id_contract = {IDContract} where d.id_car = c.id_car";
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
        }
    }
}
