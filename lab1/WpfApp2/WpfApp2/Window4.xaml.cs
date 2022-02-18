using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        static List<Student> DataBase = new();
        public Window4()
        {
            InitializeComponent();
            CreateData();
        }
        public void CreateData()
        {
            Data.Text = "";
            StreamReader DataBaseRead;

            DataBaseRead = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\DataBase.txt");
            while (!DataBaseRead.EndOfStream)
            {
                string[] Line = DataBaseRead.ReadLine().Split(' ');
                DataBase.Add(new Student(long.Parse(Line[0]), Line[1], Line[2], Line[3]));
            }
            DataBaseRead.Close();
            if (DataBase.Count == 0)
                Data.Text = "NoElements";
            else
            {
                foreach (var s in DataBase)
                {
                    Data.Text += s.PrintStudent() + "\n";
                }
                DataBase.Clear();
            }
        }
        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            Close();
            AddStudent add = new();
            add.Show();
        }
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            Close();
            DeleteSudent del = new();
            del.Show();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new();
            mw.Show();
            Close();
        }
    }
}
