using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace CourseWork
{
    public partial class ContractsData : Window
    {
        private static String Connection = @"Data Source=helgamax\mssqlservers; Initial Catalog=CarsDB;Integrated Security=True";
        SqlConnection sqlConn;
        DataTable dT1;
        DateTime startDate;
        DateTime endDate;
        int startComission;
        int endComission;
        int startPrice;
        int endPrice;
        string IDContract;
        public ContractsData()
        {
            InitializeComponent();
            Annulate();
            ShowTable();
        }

        private void Annulate()
        {
            startDate = new DateTime(1999, 1, 1);
            endDate = new DateTime(2100, 1, 1);
            startComission = 0;
            endComission = 5000000;
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
            try { startDate = DateTime.Parse(StartDate.Text); } catch { }
            try { endDate = DateTime.Parse(EndDate.Text); } catch { }
            try { startComission = Int32.Parse(ComStart.Text); } catch { }
            try { endComission = Int32.Parse(ComEnd.Text);}catch { }
            try{ startPrice = Int32.Parse(PriceStart.Text);}catch { }
            try{endPrice = Int32.Parse(PriceEnd.Text);}catch { }
            ShowTable();
        }
        private void ShowTable()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            string command = "";

            command = $"SELECT id_contract as[ID], date_contract as [Дата підписання контракту], date_sell as[Дата продажу], price as[Ціна авто]," +
                $" comission as [Комісія], note as[Примітка], contract as [Шлях до контракту]" +
                $" FROM  contracts WHERE (date_contract BETWEEN '{startDate}' AND '{endDate}') " +
                $"AND (price>={startPrice} AND price <= {endPrice}) AND (comission>={startComission} AND comission<={endComission})";
            if (SpecialNote.Text == "-")
            {
                command += $" AND note = ''";
            }
            else if (SpecialNote.Text != "")
            {
                command += $" AND note = '{SpecialNote.Text}'";
            }
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                var Data = new SqlDataAdapter(command, sqlConn);
                dT1 = new DataTable("contracts");

                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
            }
            sqlConn.Close();
            Annulate();
        }

        private void OpenContract_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                ShowContract s = new ShowContract();
                Close(); s.Show();
                s.ShowData(IDContract, false);
            }
        }

        private void DataGrid1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedItem != null)
            {
                IDContract = dT1.Rows[DataGrid1.SelectedIndex][0].ToString();
                label.Content = "Покупець: " + getPersonByID("id_customer", "customers");
                label2.Content = "Дилер: " + getPersonByID("id_dealer", "dealers");
                label3.Content = "Власник авто: " + getOwnerByID();
            }
        }
        private string getPersonByID(string idName, string TableName)
        {
            string person = "";
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = $"select {TableName}.{idName}, surname, name, patronymic from {TableName} , contracts where " +
                    $"{TableName}.{idName}= contracts.{idName} and contracts.id_contract = {IDContract}";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                reader.Read();
                person = $"({reader[0]}) {reader[1]} {reader[2]} {reader[3]}";
                reader.Close();
                sqlConn.Close();
            }
            return person;
        }
        private string getOwnerByID()
        {
            string person = "";
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                string request = $"select a.id_owner,surname, a.name, patronymic from owners a, automobiles b," +
                    $" contracts c where a.id_owner = b.id_owner and b.id_car = c.id_car and c.id_contract ={IDContract}";
                var sqlCommand = new SqlCommand(request, sqlConn);
                var reader = sqlCommand.ExecuteReader();
                reader.Read();
                person = $"({reader[0]}) {reader[1]} {reader[2]} {reader[3]}";
                reader.Close();
                sqlConn.Close();
            }
            return person;
        }
    }
}
