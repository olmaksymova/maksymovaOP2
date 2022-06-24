using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
namespace Prj_Soft_Protection
{
    /// <summary>
    /// Логика взаимодействия для StudyModeWindow.xaml
    /// </summary>
    public partial class StudyModeWindow : Window
    {
        static int IdentificationCount;
        static int SymbolsCount = 0;
        static List<double> Delays = new List<double>();
        static TimeSpan Delaying;
        static StreamWriter Data;
        static Stopwatch TimeOfDelays = new Stopwatch();
        public StudyModeWindow()
        {
            SymbolsCount = 0;
            IdentificationCount = 0;
            InitializeComponent();

            IdentificationCount = 0;
            IdentificationCount = Convert.ToInt32(CountProtection.Text);
            VerifField.Text = Coefficient.IdentifPhrase;
            Data = new StreamWriter(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\Prj_Soft_Protection\Data.txt");

            TimeOfDelays.Reset();
        }
    
        private void CloseStudyMode_Click(object sender, RoutedEventArgs e)
        {
            TimeOfDelays.Stop();
            MainWindow mw = new MainWindow();
            mw.Show();
            Hide();
            Data.Close();
        }

        private void IF_KeyDown(object sender, KeyEventArgs e)
        {
            if (SymbolsCount == 0 || InputField.Text == "")
            {
                TimeOfDelays.Start();
                SymbolsCount++;

                SymbolCount.Content = SymbolsCount;

            }
            else if (!(SymbolsCount == Coefficient.IdentifPhrase.Length))
            {
                TimeOfDelays.Stop();
                Delaying = TimeOfDelays.Elapsed;
                Delays.Add(Delaying.TotalMilliseconds / 1000);
                SymbolsCount++;
                SymbolCount.Content = SymbolsCount;
                TimeOfDelays.Reset();
                TimeOfDelays.Start();
            }
        }

        private void CountProtection_DropDownClosed(object sender, EventArgs e)
        {
            if (CountProtection.Text != "")
            {
                IdentificationCount = Int32.Parse(CountProtection.Text);
                InputField.IsEnabled = true;
            }
        }

        private void InputField_KeyUp(object sender, KeyEventArgs e)
        {
            if (InputField.Text.Length == Coefficient.IdentifPhrase.Length)
            {
                TimeOfDelays.Reset();
                if (InputField.Text == Coefficient.IdentifPhrase)
                {
                    foreach (var str in Delays)
                    {
                        Data.Write(str + " ");
                    }
                    Data.WriteLine();

                    Delays.Clear();
                }
                else
                    MessageBox.Show("Помилка введення!");
                InputField.Text = "";
                SymbolCount.Content = "0";

                Delays.Clear();
                SymbolsCount = 0;

                IdentificationCount--;
                if (IdentificationCount == 0)
                {
                    InputField.IsEnabled = false;
                }
            }
        }
    }
}
