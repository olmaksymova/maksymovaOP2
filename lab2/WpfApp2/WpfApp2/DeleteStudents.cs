using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WpfApp2
{
    class DeleteStudents
    {
        public DeleteStudents()
        {
            InitMe();
        }

        Window WN;
        Grid LayoutRoot;
        TextBox id;

        private void InitMe()
        {
            WN = ECreate.WNCreate(300, 170, "Видалення студента");
            LayoutRoot = ECreate.GridCreate(WN.Width, WN.Height);
            id = ECreate.CreateTBox(100, 30, 70, 60, 18, new FontFamily("Segoe UI"), LayoutRoot);

            TextBlock DeleteStudent = ECreate.CreateTBlock(WN.Width, 45, 0, 20, 20, "Видаліть студента за ID", new FontFamily("Century Gothic"), LayoutRoot);
            DeleteStudent.TextAlignment = TextAlignment.Center;
            TextBlock IDLabel = ECreate.CreateTBlock(50, 45, 25, 60, 20, "ID:", new FontFamily("Century Gothic"), LayoutRoot);

            Button ToMainWindow = ECreate.ButtonCreate(30, 100, IDLabel.Height + 60, 155, "Назад", Brushes.Salmon, LayoutRoot);
            Button OK = ECreate.ButtonCreate(30, 100, IDLabel.Height + 60, 20, "OK", Brushes.LightGreen, LayoutRoot);

            OK.Click += OK_Click;
            ToMainWindow.Click += ToMainWindow_Click;

            WN.Content = LayoutRoot;
            WN.Show();
        }

        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            WN.Hide();
            StudentsDB DB = new();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {

            StreamReader DataBaseRead = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab2\WpfApp2\DataBase.txt");

            List<Student> DataBase = new();
            while (!DataBaseRead.EndOfStream)
            {
                string[] Line = DataBaseRead.ReadLine().Split(' ');
                DataBase.Add(new Student(long.Parse(Line[0]), Line[1], Line[2], Line[3]));
            }
            DataBaseRead.Close();
            if (DataBase.FindAll(a => a.getID().ToString() == id.Text).Count != 0)
            {
                FileStream file = new FileStream(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab2\WpfApp2\DataBase.txt", FileMode.Create, FileAccess.Write);
                file.SetLength(0);
                file.Close();

                StreamWriter DataBaseWrite = new StreamWriter(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab2\WpfApp2\DataBase.txt");
                DataBase.Remove(DataBase.Find(a => a.getID() == long.Parse(id.Text)));

                foreach (var s in DataBase)
                    DataBaseWrite.WriteLine(s.PrintStudent());

                DataBaseWrite.Close();
            }
            else
            {
                MessageBox.Show("Даного студента в базі немає!");
            }
        }
    }
}

