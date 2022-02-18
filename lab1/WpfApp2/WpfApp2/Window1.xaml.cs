using System.Collections.Generic;
using System.Windows;
using static System.Math;
namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        List<string> Operations = new();
        const string opers = "+-*/";
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Hide();
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
                if (Cal.Text.Length > 0)
                    if (Cal.Text[^1] == '0')
                        Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1);
            }
            else
                Operations[^1] = Operations[^1] + num;
            Cal.Text += num;
        }
        public void AddOperation(string oper)
        {
            if ((Operations.Count == 3 && Operations[0] != "-") || Operations.Count == 4)
            {
                double result = Round(Calculate(), 4);
                Cal.Text = result.ToString();
                Operations.Add(result.ToString());
            }
            if (Operations.Count > 0)
                if (opers.Contains(Operations[^1]))
                {
                    Operations[^1] = oper;
                    Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1) + oper;
                }
                else
                {
                    Operations.Add(oper);
                    Cal.Text += oper;
                }
        }
        private void One_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("1");
        }

        private void Two_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("2");
        }

        private void Three_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("3");
        }

        private void Four_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("4");
        }

        private void Five_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("5");
        }

        private void Six_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("6");

        }

        private void Seven_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("7");
        }

        private void Eight_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("8");
        }

        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("9");
        }

        private void Zero_Click(object sender, RoutedEventArgs e)
        {
            AddNumber("0");
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            AddOperation("-");
        }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            AddOperation("+");
        }

        private void Multiple_Click(object sender, RoutedEventArgs e)
        {
            AddOperation("*");
        }

        private void Divide_Click(object sender, RoutedEventArgs e)
        {
            AddOperation("/");
        }

        private void Dot_Click(object sender, RoutedEventArgs e)
        {
            AddNumber(",");
        }

        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            if (Operations.Count > 1)
            {
                if (Cal.Text[^1] == '+')
                {
                    Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1);
                    Cal.Text += "-";
                    Operations[^1] = "-";
                }
                else if (Cal.Text[^1] == '-')
                {
                    Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1);
                    Cal.Text += "+";
                    Operations[^1] = "-";
                }
                else if (Operations[^2] == "-")
                {
                    Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1 - Operations[^1].Length);
                    Cal.Text += "+" + Operations[^1];
                    Operations[^2] = "+";
                }
                else if (Operations[^2] == "+")
                {
                    Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1 - Operations[^1].Length);
                    Cal.Text += "-" + Operations[^1];
                    Operations[^2] = "-";
                }
                else if (!opers.Contains(Operations[^1]))
                {
                    if (double.Parse(Operations[^1]) > 0)
                    {
                        Cal.Text = "";
                        Operations[^1] = "-" + Operations[^1];
                        for (int i = 0; i < Operations.Count - 1; i++)
                            Cal.Text += Operations[i];
                        Cal.Text += "(" + Operations[^1] + ")";
                    }
                    else if (double.Parse(Operations[^1]) < 0)
                    {
                        Cal.Text = "";
                        Operations[^1] = (double.Parse(Operations[^1]) * (-1)).ToString();
                        for (int i = 0; i < Operations.Count; i++)
                            Cal.Text += Operations[i];
                    }
                }
            }
            else if (Operations.Count == 1)
            {
                Cal.Text = "";
                Operations[0] = (double.Parse(Operations[^1]) * (-1)).ToString();
                Cal.Text += Operations[0];
            }
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            double result = Round(Calculate(), 4);
            Cal.Text = result.ToString();
            Operations.Add(result.ToString());
        }

        private void Erase_Click(object sender, RoutedEventArgs e)
        {
            Operations.Clear();
            Cal.Text = "0";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Operations.Count > 0)
            {
                if (Operations[^1].Length > 1)
                    Operations[^1] = Operations[^1].Remove(Operations[^1].Length - 1);
                else
                    Operations.RemoveAt(Operations.Count - 1);
                if (Cal.Text.Length > 0)
                    Cal.Text = Cal.Text.Remove(Cal.Text.Length - 1);
            }
        }
    }
}
