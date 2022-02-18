using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для DeleteSudent.xaml
    /// </summary>
    public partial class DeleteSudent : Window
    {
        public DeleteSudent()
        {
            InitializeComponent();
        }

        private void Ok_Click_1(object sender, RoutedEventArgs e)
        {


            StreamReader DataBaseRead = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\DataBase.txt");

            List<Student> DataBase = new();
            while (!DataBaseRead.EndOfStream)
            {
                string[] Line = DataBaseRead.ReadLine().Split(' ');
                DataBase.Add(new Student(long.Parse(Line[0]), Line[1], Line[2], Line[3]));
            }
            DataBaseRead.Close();
            if (DataBase.FindAll(a => a.getID().ToString() == id.Text).Count != 0)
            {
                FileStream file = new FileStream(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\DataBase.txt", FileMode.Create, FileAccess.Write);
                file.SetLength(0);
                file.Close();

                StreamWriter DataBaseWrite = new StreamWriter(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\DataBase.txt");
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Window4 w4 = new();
            w4.Show();
        }
    }
}
