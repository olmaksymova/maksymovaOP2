using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
namespace WpfApp2
{
    class AddData
    {
        public AddData()
        {
            InitMe();
        }

        Window WN;
        Grid LayoutRoot;
        TextBox id, firstname, surname, group;
        TextBlock ID, FirstName, SurName, Group;

        private void InitMe()
        {
            WN = ECreate.WNCreate(320, 310, "Додати студента");
            LayoutRoot = ECreate.GridCreate(WN.Width, WN.Height);

           Button ToMainWindow = ECreate.ButtonCreate(30, 100, 250, 155, "Назад", Brushes.Salmon, LayoutRoot);
           Button OK = ECreate.ButtonCreate(30, 100, 250, 20, "OK", Brushes.LightGreen, LayoutRoot);
     
            TextBlock AddStudent = ECreate.CreateTBlock(WN.Width, 45, 0, 20, 20, "Додайте студента за ID", new FontFamily("Century Gothic"), LayoutRoot);
            AddStudent.TextAlignment = TextAlignment.Center;

            ID = ECreate.CreateTBlock(100,30,30, 60,16, "ID",new FontFamily("Segoe UI"),LayoutRoot);
            FirstName = ECreate.CreateTBlock(100, 30,ID.Margin.Left, 50 * 2 + ID.Margin.Top,16, "Ім'я", new FontFamily("Segoe UI"), LayoutRoot);
            SurName = ECreate.CreateTBlock(100, 30, ID.Margin.Left, 50 + ID.Margin.Top, 16, "Прізвище", new FontFamily("Segoe UI"), LayoutRoot);
            Group = ECreate.CreateTBlock(100, 30, ID.Margin.Left, 50 * 3 + ID.Margin.Top, 16, "Група", new FontFamily("Segoe UI"), LayoutRoot);

            id = ECreate.CreateTBox(173,30,110, 60, 16, new FontFamily("Segoe UI"), LayoutRoot);
            surname = ECreate.CreateTBox(173, 30, id.Margin.Left, 50 + id.Margin.Top, 16, new FontFamily("Segoe UI"), LayoutRoot);
            firstname = ECreate.CreateTBox(173, 30, id.Margin.Left, 100 + id.Margin.Top, 16, new FontFamily("Segoe UI"), LayoutRoot);
            group = ECreate.CreateTBox(173, 30, id.Margin.Left, 150 + id.Margin.Top, 16, new FontFamily("Segoe UI"), LayoutRoot);
            
            OK.Click += Ok_Click;
            ToMainWindow.Click += ToMainWindow_Click;

            WN.Content = LayoutRoot;
            WN.Show();
        }
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            WN.Hide();
            StudentsDB DB = new();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            StudentsDB DB = new();
            WN.Hide();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool SameStudent = false;
                StreamReader DataBaseRead;
                DataBaseRead = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab2\WpfApp2\DataBase.txt");
                List<string> lines = new();
                string[] line = new string[4];
                while (!DataBaseRead.EndOfStream)
                {
                    lines.Add(DataBaseRead.ReadLine());
                    line = lines[^1].Split(' ');
                    if (line[0] == id.Text)
                    {
                        MessageBox.Show("Студент з таким ID вже є в базі!");
                        SameStudent = true;
                        break;
                    }
                }
                DataBaseRead.Close();
                if (!SameStudent)
                {
                    Student St = new Student(long.Parse(id.Text), firstname.Text, surname.Text, group.Text);
                    StreamWriter DataBaseWrite = new StreamWriter(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab2\WpfApp2\DataBase.txt");

                    foreach (var a in lines)
                        DataBaseWrite.WriteLine(a);

                    DataBaseWrite.WriteLine(St.PrintStudent());
                    DataBaseWrite.Close();
                    MessageBox.Show("Дані успішно додано в базу!", "Успіх");

                    foreach (var t in LayoutRoot.Children)
                        if (t is TextBox)
                            ((TextBox)t).Text = "";
                }
            }
            catch
            {
                MessageBox.Show("Помилка введення даних!", "ПОМИЛКА");
            }
        }
    }
}