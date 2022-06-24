using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using static System.Math;
namespace Prj_Soft_Protection
{
    /// <summary>
    /// Логика взаимодействия для ProtectionModeWindow.xaml
    /// </summary>
    public partial class ProtectionModeWindow : Window
    {
        StreamReader Data;

        string DataText;

        List<double> TimeStops;
        static int SymbolsCount = 0;

        static int IdentificationCount;
        double alpha = 0.2;
        int n;

        static List<double> CurrentUserDelays;
        static List<double> Delays;

        static Stopwatch TimeOfDelays = new Stopwatch();
        static TimeSpan Delaying;

        public ProtectionModeWindow()
        {
            TimeStops = new List<double>();
            InitializeComponent();
            IdentificationCount = 0;
            SymbolsCount = 0;
            Delays = new List<double>();
            CurrentUserDelays = new List<double>();

            n = Coefficient.IdentifPhrase.Length - 1;

            IdentificationCount = Convert.ToInt32(CountProtection.Text);
            alpha = Convert.ToDouble(AlphaSelector.Text);

            MakeCopy();
        }
        public List<double> getM(List<double> y, double n)
        {
            List<double> M = new List<double>();
            foreach (var current in y)
            {
                double sum = y.Sum() - current;
                M.Add(sum / (n - 1));
            }
            return M;
        }
        public List<double> getS(List<double> y, List<double> M, double n)
        {
            List<double> S = new List<double>();
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += (y[i] - M[i]) * (y[i] - M[i]);
            }
            for (int i = 0; i < n; i++)
            {
                double currentsum = sum - (y[i] - M[i]) * (y[i] - M[i]);
                S.Add(Sqrt(currentsum / (n - 2)));
            }
            return S;
        }
        public List<double> getT(List<double> y, List<double> M, List<double> S, double n)
        {
            List<double> T = new List<double>();
            for (int i = 0; i < n; i++)
            {
                T.Add(Abs((y[i] - M[i]) / (S[i])));
            }
            return T;
        }
        /* Перевірка значень по критерію стьюдента */
        private void RemoveWrongStops(List<double> Stops)
        {
            int countLines = Stops.Count / n;

            int k = 0;
            for (int i = 0; i < countLines; i++)
            {
                List<double> Stroke = Stops.GetRange(k * n, n);
                List<double> CurrentM = getM(Stroke, n);
                List<double> CurrentS = getS(Stroke, CurrentM, n);
                List<double> CurrentT = getT(Stroke, CurrentM, CurrentS, n);

                if (CurrentT.FindAll(a => a > Coefficient.Student6[alpha]).Count != 0)
                {
                    Stops.RemoveRange(k * n, n);
                    k--;
                }
                k++;
            }
        }

        /* Запис даних з файлу з еталонними параметрами */
        public void MakeCopy()
        {
            TimeStops.Clear();
            Data = new StreamReader(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\Prj_Soft_Protection\Data.txt");
            DataText = "";
            while (!Data.EndOfStream)
            {
                string[] Line = Data.ReadLine().Split(' ');
                if (Line.Length == n + 1)
                {
                    foreach (var a in Line)
                    {
                        try
                        {
                            TimeStops.Add(Double.Parse(a));
                            DataText += a + " ";
                        }
                        catch { };
                    }
                    DataText += "\n";
                }
            }
            if (DataText == "")
            {
                MessageBox.Show("Файл пустий! Задайте спочатку еталонні параметри!");
            }
            Data.Close();
        }
        /*Закриття поточного вікна*/
        private void CloseStudyMode_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Close();
            mw.Show();
        }
        /*Встановлення значення альфа*/
        private void AlphaSelector_DropDownClosed(object sender, EventArgs e)
        {
            if (AlphaSelector.Text != "")
            {
                alpha = Double.Parse(AlphaSelector.Text);
                if (CountProtection.Text != "")
                    IdentificationCount = Int32.Parse(CountProtection.Text);
                MakeCopy();
            }

            InputField.IsEnabled = true;
        }
        /*Встановлення кількості спроб*/
        private void CountProtection_DropDownClosed(object sender, EventArgs e)
        {
            if (CountProtection.Text != "")
                IdentificationCount = Int32.Parse(CountProtection.Text);

            InputField.IsEnabled = true;
        }
        /*Створення затримок при нажатті кнопок */
        private void InputField_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
        /*Отримання вибіркової дисперсії*/
        public List<double> getDispersion(List<double> Stops, int n)
        {
            int rows = Stops.Count / n;
            List<double> Dispersion = new List<double>();

            for (int i = 0; i < rows; i++)
            {
                List<double> TempStops = new List<double>();
                for (int j = i * n; j < n * (i + 1); j++)
                    TempStops.Add(Stops[j]);

                double sum = 0, MathExpect = TempStops.Sum() / n;

                foreach (var t in TempStops)
                    sum += Pow(t - MathExpect, 2);
                Dispersion.Add(sum / (n - 1));
            }
            return Dispersion;
        }
        /*Перевірка даних поточного користувача*/
        private void CheckUser()
        {
            RemoveWrongStops(CurrentUserDelays);
            RemoveWrongStops(TimeStops);

            int CurUserRows = CurrentUserDelays.Count / n;
            int AdminRows = TimeStops.Count / n;
            if (CurUserRows != 0 && AdminRows != 0)
            {
                List<double> AdminDispersion = getDispersion(CurrentUserDelays, n);
                List<double> UserDispersion = getDispersion(TimeStops, n);

                double fisherCoeff = Coefficient.Fisher7[alpha];
                double StudentCoeff = Coefficient.Student7[alpha];
                int success = 0, error = 0, coincedence = 0, counter = 0, check;
                var Writer = new StreamWriter(@"C:\Users\olyam\OneDrive\Рабочий стол\кпи\оп 2\Prj_Soft_Protection\Data.txt");
                Writer.Write(DataText);

                foreach (var d in UserDispersion)
                {
                    check = coincedence;
                    foreach (var adm in AdminDispersion)
                    {
                        double currentF = Max(d, adm) / Min(d, adm);
                        if (currentF > fisherCoeff)
                            error++;
                        else
                            success++;

                        double S = Sqrt((d + adm) * (n - 1) / (2 * n - 1));
                        double currentStudent = Abs((TimeStops.Sum() - CurrentUserDelays.Sum()) / n) / (S * Sqrt(2 / n));
                        if (currentF <= StudentCoeff)
                        {
                            coincedence++;
                        }
                    }
                    if (coincedence > check && counter < CurUserRows)
                    {
                        /*Запис даних успішної автентифікації в файл*/
                        for (int i = counter * n; i < n * (counter + 1); i++)
                        {
                            Writer.Write(CurrentUserDelays[i] + " ");
                        }
                        Writer.WriteLine();
                    }
                    counter++;
                }
                Writer.Close();
                /*Запис результатів у вікно*/
                if (Admin.IsChecked == true)
                {
                    P1Field.Content = (double)error / (CurUserRows * AdminRows);
                    P2Field.Content = "";
                }
                else
                {
                    P2Field.Content = (double)success / (CurUserRows * AdminRows);
                    P1Field.Content = "";
                }
                if (success > error)
                    DispField.Content = "однорідні";
                else
                    DispField.Content = "неоднорідні";
                StatisticsBlock.Content = (double)coincedence / (CurUserRows * AdminRows);
            }
            else
                MessageBox.Show("Замало даних для перевірки!");
        }
        /*Запис даних поточного користуача до списку та оновлення вікна*/
        private void InputField_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (InputField.Text.Length == Coefficient.IdentifPhrase.Length)
            {
                TimeOfDelays.Reset();
                if (InputField.Text == Coefficient.IdentifPhrase)
                {
                    foreach (var str in Delays)
                    {
                        CurrentUserDelays.Add(str);
                    }
                    Delays.Clear();
                }
                else
                    MessageBox.Show("Помилка введення даних!");
                InputField.Text = "";
                SymbolCount.Content = "0";

                Delays.Clear();
                SymbolsCount = 0;

                IdentificationCount--;
                if (IdentificationCount == 0)
                {
                    InputField.IsEnabled = false;
                    CheckUser();
                }
            }
        }
    }
}
