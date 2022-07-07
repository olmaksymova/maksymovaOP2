using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWork
{
    /// <summary>
    /// Логика взаимодействия для ShowContract.xaml
    /// </summary>
    public partial class ShowContract : Window
    {
        public ShowContract()
        {
            InitializeComponent();

        }
        bool a = false;
        public void ShowData(string id,bool c)
        {
            try
            {
                string path = $@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\CourseWork\contracts\{id}.txt";
                StreamReader reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    contText.Text += reader.ReadLine() + "\n";
                }
                reader.Close();
            }
            catch
            {
                MessageBox.Show("Error. Can't open file!");
            }
            a = c;
        }
        private void ToBackWindow_Click(object sender, RoutedEventArgs e)
        {
            if (a)
            {
                DealerSelect d = new DealerSelect();
                d.Show();
            }
            else
            {
                ContractsData data = new ContractsData();
                data.Show();
            }
                this.Close();
        }
    }
}
