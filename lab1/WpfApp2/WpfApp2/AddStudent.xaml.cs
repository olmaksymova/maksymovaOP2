using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        public AddStudent()
        {
            InitializeComponent();
            // Height="235" Width="329"
            Height = 250;
            Width = 333;
            foreach (var t in LayoutRoot.Children)
                if(t is TextBox)
                ((TextBox)t).TextAlignment = TextAlignment.Center; 
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Window4 w4 = new();
            w4.Data.Text = "";
            w4.CreateData();
            w4.Show();
            Hide();
        }

        private void Ok_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                bool SameStudent = false;
                StreamReader DataBaseRead;
                DataBaseRead = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\DataBase.txt");
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
                    Student St = new Student(long.Parse(id.Text), firstname.Text, secondname.Text, group.Text);

                    StreamWriter DataBaseWrite = new StreamWriter(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\lab1\WpfApp2\DataBase.txt");

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
