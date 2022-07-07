using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace CourseWork
{
    public partial class EditCustomers : Window
    {
        private static String Connection = @"Data Source=helgamax\mssqlservers; Initial Catalog=CarsDB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com, ComAddImage;
        SqlConnection sqlConn;
        OpenFileDialog op;
        DataTable dT1;
        String ID_owner = "1";
        String strQ;
        int ColumnCount;
        int start = 1, end;
        TextBox[] Values;
        String ImageWay = "";
        Dictionary<string, string> CBBoxToTable = new Dictionary<string, string> {
            {"Автомобілі","automobiles"},
            {"Дилери", "dealers"},
            {"Власники авто","owners"},
            {"Покупці", "customers"},
            {"Контракти","contracts"} };
        string TableName = "";
        List<string> Headers = new List<string>();
        public EditCustomers()
        {
            InitializeComponent();

            TableName = CBBoxToTable[MainWindow.CurrentTable];

            editTable.Text = "Редагування таблиці \"" + MainWindow.CurrentTable + "\"";
            if (TableName == "automobiles")
            {
                CBoxItems.ItemsSource = GetItems("owners");
                CBoxItems.SelectedIndex = 0;
            }
            else
            {
                CBoxItems.ItemsSource = GetItems("automobiles");
                CBoxItems.SelectedIndex = 0;
                Dealers.ItemsSource = GetItems("dealers");
                Dealers.SelectedIndex = 0;
                Customer.ItemsSource = GetItems("customers");
                Customer.SelectedIndex = 0;
            }
            ShowTable();

            ColumnCount = dT1.Columns.Count;
            int x = 162, y = 130;
            int xLabel = 38;
            for (int i = 0; i < ColumnCount; i++)
            {
                Headers.Add(dT1.Columns[i].ColumnName);
            }
            if (TableName == "contracts")
            {
                CBoxItems.Visibility = Visibility.Visible;
                Customer.Visibility = Visibility.Visible;
                Dealers.Visibility = Visibility.Visible;
                first.Visibility = Visibility.Visible;
                second.Visibility = Visibility.Visible;
                third.Visibility = Visibility.Visible;
                y += 120;
            }
            else if (TableName == "automobiles")
            {
                first.Content = "Власник авто";
                first.Visibility = Visibility.Visible;
                CBoxItems.Visibility = Visibility.Visible;
                y += 38;
            }
            int j = 1;
            switch (TableName)
            {
                case "automobiles":
                    end = 6;
                    break;
                case "contracts":
                    end = 9;
                    start = 4;
                    j = 4;
                    break;
                case "customers":
                    end = 7;
                    break;
                case "owners":
                    end = 5;
                    break;
                case "dealers":
                    end = 7;
                    start = 1;
                    break;
                default:
                    end = 6;
                    break;
            }
            ColumnCount = end - start;
            Values = new TextBox[ColumnCount];
            var Labels = new TextBlock[ColumnCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                Values[i] = new TextBox
                {
                    Name = $"TextBox{i}",
                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(x, y, 0, 0),
                    Text = "",
                    TextWrapping = TextWrapping.Wrap,
                    Width = 200,
                    Height = 28,
                    FontSize = 16,
                    FontFamily = new System.Windows.Media.FontFamily("Georgia")
                };

                Labels[i] = new TextBlock
                {
                    TextAlignment = TextAlignment.Left,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(xLabel, y + 2, 0, 0),
                    Text = Help.ColumnNames[Headers[j + i]],
                    TextWrapping = TextWrapping.Wrap,
                    Height = 20,
                    FontSize = 16,
                    FontFamily = new System.Windows.Media.FontFamily("Georgia"),
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White
                };
                y += 47;
                LayoutRoot.Children.Add(Values[i]);
                LayoutRoot.Children.Add(Labels[i]);
            }
            if (TableName == "automobiles" || TableName == "dealers")
            {
                CarPhoto.Source = new BitmapImage(new Uri(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\CourseWork\CourseWork\DefImage.png"));
                CarPhoto.Stretch = Stretch.Uniform;
                CarPhoto.MouseLeftButtonDown += Image_MouseDown;
            }
            switch (TableName)
            {
                case "automobiles":
                    Values[2].PreviewTextInput += IDField_PreviewTextInput;
                    Values[3].PreviewTextInput += IDField_PreviewTextInput;
                    Values[4].PreviewTextInput += IDField_PreviewTextInput;
                    Values[2].MaxLength = 4;
                    break;
                case "contracts":
                    Values[0].KeyUp += TextBox_KeyDown;
                    Values[1].KeyUp += TextBox_KeyDown;
                    Values[0].MaxLength = 10;
                    Values[1].MaxLength = 10;
                    Values[2].PreviewTextInput += IDField_PreviewTextInput;
                    Values[3].PreviewTextInput += IDField_PreviewTextInput;
                    break;
                case "owners":
                    CarPhoto.Stretch = Stretch.Uniform;
                    break;
            }
            double TopMargin = Values[Values.Length - 1].Height + Values[Values.Length - 1].Margin.Top + 25;
            double Height = Backgr.Height - Values[Values.Length - 1].Margin.Top - 10;
            double Width = Backgr.Width - 20;
            CarPhoto.Margin = new Thickness(CarPhoto.Margin.Left, TopMargin, 0, 0);
            CarPhoto.Height = Height;
            CarPhoto.Width = Width;
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back | e.Key == Key.Space)
            {
                e.Handled = true;
            }
            else
            {
                TextBox t = (TextBox)sender;
                if (t.Text.Length == 2 || t.Text.Length == 5)
                    t.Text = t.Text + "/";
                ((TextBox)sender).CaretIndex = t.Text.Length;
            }
        }
        private String[] GetItems(string table)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            String[] Items = { "" };
            var nums = new List<int>();

            List<int> AvailableCars = new List<int>();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter($"SELECT * FROM {table}", sqlConn);

                dT1 = new DataTable($"{TableName}");

                Data.Fill(dT1);
                Items = new String[dT1.Rows.Count];

                for (int i = 0; i < dT1.Rows.Count; i++)
                {
                    Items[i] = dT1.Rows[i][0].ToString() +
                    " " + dT1.Rows[i][1].ToString() + " " +
                    dT1.Rows[i][2].ToString();

                    AvailableCars.Add(Int32.Parse(dT1.Rows[i][0].ToString()));
                }
                if (table == "automobiles")
                {
                    string request = "select id_car from contracts";
                    var sqlCommand = new SqlCommand(request, sqlConn);
                    var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        nums.Add(Int32.Parse(reader[0].ToString()));
                    }
                    reader.Close();
                    for (int i = 0; i < Items.Length; i++)
                    {
                        int current = Int32.Parse(Items[i].Split(' ')[0]);
                        if (nums.Contains(current))
                        {
                            AvailableCars.Remove(current);
                        }
                    }

                    Items = new String[AvailableCars.Count];
                    int count = 0;
                    for (int i = 0; i < dT1.Rows.Count; i++)
                    {
                        if (AvailableCars.Contains(Int32.Parse(dT1.Rows[i][0].ToString())))
                        {
                            Items[count] = dT1.Rows[i][0].ToString() +
                            " " + dT1.Rows[i][1].ToString() + " " +
                            dT1.Rows[i][2].ToString();
                            count++;
                        }
                    }
                }
            }

            sqlConn.Close();
            return Items;
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
                var Data = new SqlDataAdapter($"SELECT * FROM {TableName}",
                 sqlConn);

                dT1 = new DataTable($"{TableName}");

                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
            }
            sqlConn.Close();
        }
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    String ID;

                    Data = new SqlDataAdapter($"SELECT * FROM   {TableName}", sqlConn);
                    dT1 = new DataTable(TableName);

                    Data.Fill(dT1);
                    ID = IDField.Text;

                    strQ = $"DELETE FROM {TableName} WHERE {Headers[0]} = '" + ID + "';";

                    if (TableName == "contracts")
                        Help.DeleteContract(ID);

                    Com = new SqlCommand(strQ, sqlConn);
                    MessageBox.Show(Com.ExecuteNonQuery().ToString());

                    sqlConn.Close();
                    ShowTable();

                    if (TableName == "contracts")
                        CBoxItems.ItemsSource = GetItems("automobiles");
                }
                catch { }
            }
        }
        private void IDField_PreviewTextInput(object sender,
        TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();

            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                String lastID;
                string OwnerID;
                Data = new SqlDataAdapter($"SELECT * FROM {TableName}", sqlConn);

                dT1 = new DataTable(TableName);
                Data.Fill(dT1);

                string command = $"INSERT INTO {TableName} (";
                if (TableName == "owners" || TableName == "dealers" || TableName == "customers" || TableName == "contracts")
                    end--;
                for (int i = 0; i < end; i++)
                {
                    command += Headers[i] + ", ";
                }

                command += $"{Headers[end]}";

                command += ")";
                if (dT1.Rows.Count > 0)
                    lastID = (1 + Convert.ToInt32(dT1.Rows[dT1.Rows.Count - 1][0])).ToString();
                else
                    lastID = "1";
                OwnerID = ID_owner;
                strQ = "";
                strQ += command + "values('" + lastID + "','";
                if (TableName == "contracts")
                {
                    strQ += $"{Customer.Text.Split(' ')[0]}', '{CBoxItems.Text.Split(' ')[0]}', '{Dealers.Text.Split(' ')[0]}','";
                }
                int size = Values.Length;
                for (int i = 0; i < size; i++)
                {
                    if (Values[i] != null)
                        strQ += Values[i].Text + "'";
                    if (i != size - 1)
                        strQ += ", '";
                }
                if (TableName == "automobiles")
                    strQ += ",'" + CBoxItems.Text.Split(' ')[0] + "'";
                strQ += ")";
                var Com = new SqlCommand(strQ, sqlConn);
                MessageBox.Show(Com.ExecuteNonQuery().ToString());
                if (ImageWay != "" && (TableName == "automobiles" || TableName == "dealers"))
                {
                    strQ = $"UPDATE {TableName} SET {Headers[Headers.Count - 1]} = (SELECT * FROM OPENROWSET(BULK '" + @ImageWay + $"', SINGLE_BLOB) AS image) WHERE {Headers[0]} = " + @lastID + ";";
                    MessageBox.Show(strQ);

                    ComAddImage = new SqlCommand(strQ, sqlConn);
                    MessageBox.Show(ComAddImage.ExecuteNonQuery().ToString());

                }
                if (TableName == "contracts")
                {
                    try
                    {
                        var GetBuyerName = new SqlCommand($"SELECT surname,name,patronymic,address FROM customers where id_customer = {Customer.Text.Split(' ')[0]}", sqlConn);
                        SqlDataReader reader = GetBuyerName.ExecuteReader();
                        reader.Read();
                        (string, string) CustName = (reader[0] + " " + reader[1] + " " + reader[2], "" + reader[3]);
                        reader.Close();

                        string[] CAR = CBoxItems.Text.Split(' ');
                        var GetOwnerName = new SqlCommand($"select  surname,name,patronymic from owners where id_owner in (select id_owner from automobiles where id_car = {CAR[0]}) ", sqlConn);
                        reader = GetOwnerName.ExecuteReader();
                        reader.Read();
                        string OwnerName = reader[0] + " " + reader[1] + " " + reader[2];
                        reader.Close();

                        var GetCar = new SqlCommand($"select name, year from automobiles where id_car = {CAR[0]}", sqlConn);
                        reader = GetCar.ExecuteReader();
                        reader.Read();
                        string CarNameYear = "\"" + reader[0] + "\"" + reader[1];
                        reader.Close();
                        var DealersNames = Dealers.Text.Split(' ');
                        string Path = Help.CreateContract(int.Parse(lastID), CustName, OwnerName, CarNameYear, DateTime.Parse(Values[0].Text).Year, DealersNames[1] + " " +
                              DealersNames[2], Double.Parse(Values[2].Text));
                        string CarPriceUpdate = $"update automobiles set price ={Values[2].Text} where id_car = {CAR[0]}";
                        var Command = new SqlCommand(CarPriceUpdate, sqlConn);
                        Command.ExecuteNonQuery();

                        CBoxItems.ItemsSource = GetItems("automobiles");
                    }
                    catch (Exception t) { MessageBox.Show(t.Message); };
                }
                if (TableName == "owners" || TableName == "dealers" || TableName == "customers" || TableName == "contracts")
                    end++;
                sqlConn.Close();
                ShowTable();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
            "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                CarPhoto.Source = new BitmapImage(new Uri(op.FileName));

                ImageWay = op.FileName;
            }
        }
        private void CBoxItems_SelectionChanged(object sender,
        SelectionChangedEventArgs e)
        {
            try
            {
                if (CBoxItems.SelectedItem == null)
                    CBoxItems.SelectedIndex = 0;
                String Text = CBoxItems.SelectedItem.ToString();

                for (int i = 0; i < dT1.Rows.Count; i++)
                {
                    String tempStr = dT1.Rows[i][1].ToString() + " " + dT1.Rows[i][2].ToString() + " " + dT1.Rows[i][3].ToString();

                    if (Text == tempStr)
                        ID_owner = dT1.Rows[i][0].ToString();
                }
            }
            catch { }
        }
        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            String Redact_Index;

            DataGridColumn EditedCol = e.Column;
            Int32 Col = EditedCol.DisplayIndex;
            TextBox t = e.EditingElement as TextBox;
            String editedCellValue = t.Text.ToString();
            DataRowView row = (DataRowView)DataGrid1.SelectedItem;
            Redact_Index = row[Headers[0]].ToString();

            if (Col != 0)
                strQ = $"UPDATE {TableName} SET {Headers[Col]} = '" + editedCellValue + $"' WHERE {Headers[0]} = " + Redact_Index + ";";
            try
            {
                sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                Com = new SqlCommand(strQ, sqlConn);

                MessageBox.Show(Com.ExecuteNonQuery().ToString());
                sqlConn.Close();
            }
            catch (Exception a) { MessageBox.Show(a.ToString()); };
        }
        private void DataGrid1_MouseRightButtonUp(object sender,
        MouseButtonEventArgs e)
        {
            ShowTable();
        }
    }
}
