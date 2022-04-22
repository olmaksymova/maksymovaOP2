using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    class StudentsDB
    {
        public StudentsDB()
        {
            CreateData();
            InitDB();
        }

        Window wn = new Window();
        Grid LayoutRoot = new Grid();
        TextBlock Data = new TextBlock(), Stud;
        Button DeleteStudent, AddStudent;
        int StudentsNum = 0;
        public void InitDB()
        {
            wn = ECreate.WNCreate(514, StudentsNum * 20 + 250, "Студентська база");
            LayoutRoot = ECreate.GridCreate(wn.Width, wn.Height - 50);

            Stud = ECreate.CreateTBlock(wn.Width, 45, 0, 0, 22, "Студенти", new FontFamily("Arial Black"), LayoutRoot);
            Stud.TextAlignment = TextAlignment.Center;

            AddStudent = ECreate.ButtonCreate(45, (int)wn.Width / 2 - 50, Stud.Height + Data.Height + 10, 30, "Додати студента", Brushes.LightGreen, LayoutRoot);
            DeleteStudent = ECreate.ButtonCreate(45, (int)wn.Width / 2 - 50, Stud.Height + Data.Height + 10, 60 + AddStudent.Width, "Видалити студента", Brushes.Salmon, LayoutRoot);

            Button ToMainWindow = ECreate.ButtonCreate(45, wn.Width - 50, AddStudent.Height + Stud.Height + Data.Height + 20, 24, "Назад", Brushes.Salmon, LayoutRoot);

            ToMainWindow.Click += ToMainWindow_Click;
            DeleteStudent.Click += DeleteStudent_Click;
            AddStudent.Click += AddStudent_Click;

            LayoutRoot.Children.Add(Data);

            wn.Content = LayoutRoot;
            wn.Show();
        }
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            wn.Hide();
            MainWindow MW = new();
            MW.Show();
        }
        static List<Student> DataBase = new();
        public void CreateData()
        {
            Data = ECreate.CreateTBlock(450, wn.Height - 45 * 3, 0, 45, 16, "", new FontFamily("Arial Black"));
            Data.HorizontalAlignment = HorizontalAlignment.Center;
            Data.Background = Brushes.Beige;
            StreamReader DataBaseRead;

            DataBaseRead = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab2\WpfApp2\DataBase.txt");

            while (!DataBaseRead.EndOfStream)
            {
                string[] Line = DataBaseRead.ReadLine().Split(' ');
                DataBase.Add(new Student(long.Parse(Line[0]), Line[1], Line[2], Line[3]));
            }

            StudentsNum = DataBase.Count;
            DataBaseRead.Close();

            if (DataBase.Count == 0)
                Data.Text = "NoElements";
            else
            {
                foreach (var s in DataBase)
                    Data.Text += s.PrintStudent() + "\n";
                DataBase.Clear();
            }
            Data.Height = StudentsNum * 20 + 30;
        }
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            wn.Close();
            AddData add = new();
        }
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            wn.Close();
            DeleteStudents del = new();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new();
            mw.Show();
            wn.Close();
        }
    }
}