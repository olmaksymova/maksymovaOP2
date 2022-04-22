using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Math;

namespace WpfApp2
{
    class Calculator
    {
        public Calculator()
        {
            InitCalc();
        }
        Window wn = new Window();
        Grid LayoutRoot = new Grid();
        TextBlock NumField;

        private void InitCalc()
        {
            wn = ECreate.WNCreate(370, 420, "Калькулятор");
            wn.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
            LayoutRoot = ECreate.GridCreate(wn.Width, wn.Height-50);

            NumField = ECreate.CreateTBlock(wn.Width - 15, 45, 0, 0, 40, "0", new FontFamily("Segoe UI"));
            NumField.TextAlignment = TextAlignment.Right;
            NumField.FontWeight = FontWeights.Normal;

            int height = 48, width = 85, counter = 1;
            Button[,] KeyControls = new Button[5, 4];

            KeyControls[0, 0] = ECreate.ButtonCreate(height, width, NumField.Height + 5, 0, "", "", new SolidColorBrush(Color.FromRgb(240, 240, 240)),LayoutRoot);
            KeyControls[0, 1] = ECreate.ButtonCreate(height, width, NumField.Height + 5, width + 5, "Result", "=", new SolidColorBrush(Color.FromRgb(240, 240, 240)),LayoutRoot);
            KeyControls[0, 2] = ECreate.ButtonCreate(height, width, NumField.Height + 5, width * 2 + 10, "Erase", "C", new SolidColorBrush(Color.FromRgb(240, 240, 240)),LayoutRoot);
            KeyControls[0, 3] = ECreate.ButtonCreate(height, width, NumField.Height + 5, width * 3 + 15, "Delete", "⌫", new SolidColorBrush(Color.FromRgb(240, 240, 240)),LayoutRoot);

            KeyControls[0, 0].IsEnabled = false;

            KeyControls[0, 1].Click += Result_Click;
            KeyControls[0, 2].Click += Erase_Click;
            KeyControls[0, 3].Click += Delete_Click;

            Button ToMW = ECreate.ButtonCreate(45, wn.Width - 50, NumField.Height + 5 + 5 * (height + 5), 24, "ToMainWindow", "Назад", Brushes.Salmon, LayoutRoot);

            for (int i = 1; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    KeyControls[i, j] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + i * (height + 5), j * (width + 5), "Num" + counter.ToString(), counter.ToString(), new SolidColorBrush(Color.FromRgb(250, 250, 250)), LayoutRoot);
                    KeyControls[i, j].Click += Number_Click;
                    counter++;
                }

            KeyControls[1, 3] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 1 * (height + 5), 3 * (width + 5), "Divide", "/", new SolidColorBrush(Color.FromRgb(240, 240, 240)), LayoutRoot);
            KeyControls[2, 3] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 2 * (height + 5), 3 * (width + 5), "Multiple", "*", new SolidColorBrush(Color.FromRgb(240, 240, 240)), LayoutRoot);
            KeyControls[3, 3] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 3 * (height + 5), 3 * (width + 5), "Add", "+", new SolidColorBrush(Color.FromRgb(240, 240, 240)), LayoutRoot);
            KeyControls[4, 3] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 4 * (height + 5), 3 * (width + 5), "Minus", "-", new SolidColorBrush(Color.FromRgb(240, 240, 240)), LayoutRoot);
            KeyControls[4, 0] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 4 * (height + 5), 0, "Reverse", "(+-)", new SolidColorBrush(Color.FromRgb(250, 250, 250)), LayoutRoot);
            KeyControls[4, 1] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 4 * (height + 5), width + 5, "Num0", "0", new SolidColorBrush(Color.FromRgb(250, 250, 250)), LayoutRoot);
            KeyControls[4, 2] = ECreate.ButtonCreate(height, width, NumField.Height + 5 + 4 * (height + 5), 2 * (width + 5), "Dot", ",", new SolidColorBrush(Color.FromRgb(250, 250, 250)), LayoutRoot);
           
            for (int i = 1; i < 5; i++)
                KeyControls[i, 3].Click += Operation_Click;

            KeyControls[4, 0].Click += Reverse_Click;
            KeyControls[4, 1].Click += Number_Click;
            KeyControls[4, 2].Click += Number_Click;
            ToMW.Click += ToMainWindow_Click;

            LayoutRoot.Children.Add(NumField);
            wn.Content = LayoutRoot;
            wn.Show();
        }
       
        List<string> Operations = new();
        const string opers = "+-*/";
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            wn.Hide();
            MainWindow MW = new();
            MW.Show();
        }
        public double Calculate()
        {
            try
            {
                double num1 = double.Parse(Operations[0]);
                double num2 = double.Parse(Operations[2]);
                string op = Operations[1];
                Operations.Clear();
                return op switch
                {
                    "*" => num1 * num2,
                    "+" => num1 + num2,
                    "-" => num1 - num2,
                    "/" => num1 / num2,
                    _ => 0,
                };
            }
            catch { MessageBox.Show("ERROR!"); return 0; }
        }

        public void AddNumber(string num)
        {
            if (num != "," && (Operations.Count == 0 ||
                opers.Contains(Operations[^1])))
            {
                Operations.Add(num);
                if (NumField.Text.Length > 0)
                    if (NumField.Text[^1] == '0')
                        NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1);
            }
            else
                Operations[^1] = Operations[^1] + num;
            NumField.Text += num;
        }
        public void AddOperation(string oper)
        {
            if ((Operations.Count == 3 && Operations[0] != "-") || Operations.Count == 4)
            {
                double result = Round(Calculate(), 4);
                NumField.Text = result.ToString();
                Operations.Add(result.ToString());
            }
            if (Operations.Count > 0)
                if (opers.Contains(Operations[^1]))
                {
                    Operations[^1] = oper;
                    NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1) + oper;
                }
                else
                {
                    Operations.Add(oper);
                    NumField.Text += oper;
                }
        }
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button butt = (Button)sender;
            AddNumber(butt.Content.ToString());
        }
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Button butt = (Button)sender;
            AddOperation(butt.Content.ToString());
        }
        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            if (Operations.Count > 1)
            {
                if (NumField.Text[^1] == '+')
                {
                    NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1);
                    NumField.Text += "-";
                    Operations[^1] = "-";
                }
                else if (NumField.Text[^1] == '-')
                {
                    NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1);
                    NumField.Text += "+";
                    Operations[^1] = "-";
                }
                else if (Operations[^2] == "-")
                {
                    NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1 - Operations[^1].Length);
                    NumField.Text += "+" + Operations[^1];
                    Operations[^2] = "+";
                }
                else if (Operations[^2] == "+")
                {
                    NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1 - Operations[^1].Length);
                    NumField.Text += "-" + Operations[^1];
                    Operations[^2] = "-";
                }
                else if (!opers.Contains(Operations[^1]))
                {
                    if (double.Parse(Operations[^1]) > 0)
                    {
                        NumField.Text = "";
                        Operations[^1] = "-" + Operations[^1];
                        for (int i = 0; i < Operations.Count - 1; i++)
                            NumField.Text += Operations[i];
                        NumField.Text += "(" + Operations[^1] + ")";
                    }
                    else if (double.Parse(Operations[^1]) < 0)
                    {
                        NumField.Text = "";
                        Operations[^1] = (double.Parse(Operations[^1]) * (-1)).ToString();
                        for (int i = 0; i < Operations.Count; i++)
                            NumField.Text += Operations[i];
                    }
                }
            }
            else if (Operations.Count == 1)
            {
                NumField.Text = "";
                Operations[0] = (double.Parse(Operations[^1]) * (-1)).ToString();
                NumField.Text += Operations[0];
            }
        }
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            double result = Round(Calculate(), 4);
            NumField.Text = result.ToString();
            Operations.Add(result.ToString());
        }
        private void Erase_Click(object sender, RoutedEventArgs e)
        {
            Operations.Clear();
            NumField.Text = "0";
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Operations.Count > 0)
            {
                if (Operations[^1].Length > 1)
                    Operations[^1] = Operations[^1].Remove(Operations[^1].Length - 1);
                else
                    Operations.RemoveAt(Operations.Count - 1);
                if (NumField.Text.Length > 0)
                    NumField.Text = NumField.Text.Remove(NumField.Text.Length - 1);
            }
        }
    }
}
