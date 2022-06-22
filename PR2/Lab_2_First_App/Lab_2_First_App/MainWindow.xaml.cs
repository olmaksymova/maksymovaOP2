using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Math;
namespace Lab_2_First_App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static DispatcherTimer dT;
        static DispatcherTimer dT2;
        static int Radius = 15;
        static int SleepInterval = 1000;
        static int PointCount = 5;
        static int PopulationCount = 10;

        int[,] Parents;
        int[] MinimumWay;
        int[] Way;
        int[,] Children;
        List<int> GreedyWay = new List<int>();
        List<int> LeftPoints = new List<int>();

        static Polygon myPolygon = new Polygon();
        static List<Ellipse> EllipseArray = new List<Ellipse>();
        static PointCollection pC = new PointCollection();
        static double BreakingPoint;
        static double MutationChance = 0.7;
        static int CurrentPoint = 0;
        static int CurrentPointIndex = 0;
        public MainWindow()
        {
            dT = new DispatcherTimer();
            dT2 = new DispatcherTimer();

            InitializeComponent();
            InitPoints();
            InitPolygon();

            dT = new DispatcherTimer();
            dT.Tick += new EventHandler(OneStep);
            dT.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(VelCB.Text));

            dT2 = new DispatcherTimer();
            dT2.Tick += new EventHandler(GetGreedyWay);
            dT2.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(VelCB.Text));

            Parents = new int[PopulationCount, PointCount];
            MakingFirstGeneration();
            MyCanvas.Children.Clear();

            PlotPoints();
        }
        static private List<int> CreateNumber(int n)
        {
            List<int> Numbers = new List<int>();
            for (int i = 0; i < n; i++)
            {
                Numbers.Add(i);
            }
            return Numbers;
        }
        public int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowNumber, x]).ToArray();
        }
        private double GetAllWay(int[] Way)
        {
            double length = 0;
            for (int i = 1; i < Way.Length; i++)
            {
                length += getWay(Way[i], Way[i - 1]);
            }
            length += getWay(Way[0], Way[Way.Length - 1]);

            return length;
        }
        private int[,] FindBestWays(int[,] parents, int[,] children)
        {
            List<KeyValuePair<int, double>> Ways = new List<KeyValuePair<int, double>>();
            int[,] BestGenerations = new int[PopulationCount, PointCount];

            for (int i = 0; i < PopulationCount; i++)
            {
                Ways.Add(new KeyValuePair<int, double>(i, GetAllWay(GetRow(parents, i))));
                Ways.Add(new KeyValuePair<int, double>(i + PopulationCount, GetAllWay(GetRow(children, i))));
            }

            Ways.Sort((firstValue, nextValue) =>
            {
                return firstValue.Value.CompareTo(nextValue.Value);
            }
            );

            Random rnd = new Random();
            for (int i = 0; i < PopulationCount; i++)
            {
                int Row = Ways.ElementAt(i).Key;

                if (Row >= PopulationCount)
                {
                    for (int j = 0; j < PointCount; j++)
                        BestGenerations[i, j] = children[Row - PopulationCount, j];
                }
                else
                {
                    for (int j = 0; j < PointCount; j++)
                        BestGenerations[i, j] = parents[Row, j];
                }
            }

            return BestGenerations;
        }
        private int[,] MakingNewGeneration(int[,] parents)
        {
            int[,] children = new int[PopulationCount, PointCount];
            int ChildrenLeft = PopulationCount;
            int[] parent1, parent2, child;

            Random rnd = new Random();
            while (ChildrenLeft > 0)
            {
                BreakingPoint = rnd.NextDouble();
                int BreakingGene = (int)Math.Round(BreakingPoint * PointCount);

                if (BreakingGene == 0)
                    BreakingGene = 1;
                else if (BreakingGene == PointCount)
                    BreakingGene = PointCount - 1;

                int[] temp1 = new int[2 * PointCount];
                int[] temp2 = new int[2 * PointCount];
                child = new int[PointCount];

                int index;
                index = rnd.Next(PopulationCount);
                parent1 = GetRow(parents, index);

                int q = index;
                while (q == index)
                    index = rnd.Next(PopulationCount);
                parent2 = GetRow(parents, index);

                for (int i = 0; i < BreakingGene; i++)
                {
                    temp1[i] = parent1[i];
                    temp2[i] = parent2[i];
                }
                for (int i = BreakingGene; i < PointCount; i++)
                {
                    temp1[i] = parent2[i];
                    temp2[i] = parent1[i];
                }
                if (rnd.NextDouble() > 0.5)
                    for (int i = PointCount; i < 2 * PointCount; i++)
                        temp1[i] = temp2[i - PointCount];
                else
                {
                    for (int i = PointCount; i < 2 * PointCount; i++)
                        temp2[i] = temp1[i - PointCount];
                    temp1 = temp2;
                }

                int count = 0;
                for (int i = 0; i < 2 * PointCount; i++)
                {
                    if (!child.Contains(temp1[i]))
                    {
                        child[count] = temp1[i];
                        count++;
                        if (count == PointCount)
                            break;
                    }
                }

                index = rnd.Next(0, PointCount);
                int index2 = rnd.Next(0, PointCount);

                if (rnd.NextDouble() < MutationChance)
                {
                    Array.Reverse(child, Min(index, index2), Abs(index - index2));
                }

                for (int i = 0; i < PointCount; i++)
                {
                    children[PopulationCount - ChildrenLeft, i] = child[i];
                }
                ChildrenLeft--;
            }
            return children;
        }
        private void MakingFirstGeneration()
        {
            Parents = new int[PopulationCount, PointCount];
            Random rnd = new Random();
            for (int i = 0; i < PopulationCount; i++)
            {
                List<int> Numbers = CreateNumber(PointCount);

                for (int j = 0; j < PointCount; j++)
                {
                    int value = rnd.Next() % Numbers.Count;
                    Parents[i, j] = Numbers[value];
                    Numbers.RemoveAt(value);
                }
            }
        }
        private void InitPoints()
        {
            Random rnd = new Random();
            pC.Clear();
            EllipseArray.Clear();

            for (int i = 0; i < PointCount; i++)
            {
                Point p = new Point
                {
                    X = rnd.Next(Radius, (int)(0.75 * MainWin.Width) - 3 * Radius),
                    Y = rnd.Next(Radius, (int)(0.90 * MainWin.Height - 3 * Radius))
                };
                pC.Add(p);
            }

            for (int i = 0; i < PointCount; i++)
            {
                Ellipse el = new Ellipse
                {
                    StrokeThickness = 2
                };
                el.Height = el.Width = Radius;
                el.Stroke = Brushes.Black;
                el.Fill = Brushes.LightBlue;
                EllipseArray.Add(el);
            }
        }

        private void InitPolygon()
        {
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;
            myPolygon.StrokeThickness = 2;
        }

        private void PlotPoints()
        {
            for (int i = 0; i < PointCount; i++)
            {
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius / 2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius / 2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }
        private void PlotPoints(int index)
        {
            for (int i = 0; i < PointCount; i++)
            {
                if (i == index)
                    EllipseArray[i].Fill = Brushes.Salmon;
                Canvas.SetLeft(EllipseArray[i], pC[i].X - Radius / 2);
                Canvas.SetTop(EllipseArray[i], pC[i].Y - Radius / 2);
                MyCanvas.Children.Add(EllipseArray[i]);
            }
        }


        private void PlotWay(int[] BestWayIndex)
        {
            PointCollection Points = new PointCollection();

            for (int i = 0; i < BestWayIndex.Length; i++)
                Points.Add(pC[BestWayIndex[i]]);

            myPolygon.Points = Points;
            try
            {
                MyCanvas.Children.Add(myPolygon);
            }
            catch { };
        }

        private void VelCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;

            dT.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(item.Content));
            dT2.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt16(item.Content));
        }

        private void StopStart_Click(object sender, RoutedEventArgs e)
        {

            MainText.Content = "Пошук найменшого шляху";
            if (dT.IsEnabled)
            {
                dT.Stop();
                NumElemCB.IsEnabled = true;
                SleepCB.IsEnabled = true;
                StopStartGreedy.IsEnabled = true;
            }
            else
            {
                NumElemCB.IsEnabled = false;
                SleepCB.IsEnabled = false;
                StopStartGreedy.IsEnabled = false;
                dT.Start();
                try
                {
                    int k = Convert.ToInt32(PopCount.Text);
                    if (PopulationCount != k)
                    {
                        PopulationCount = k;
                        MakingFirstGeneration();
                    }
                }
                catch { };
                try
                {
                    MutationChance = Convert.ToDouble(MutationField.Text);
                    if (MutationChance > 1)
                    {
                        MutationChance = 1;
                        MutationField.Text = "1";
                    }
                    else if (MutationChance < 0)
                    {
                        MutationChance = 0;
                        MutationField.Text = "0";
                    }
                }
                catch { MutationChance = 0.5; };
            }
        }

        private Ellipse GetEllipse()
        {
            Ellipse el = new Ellipse
            {
                StrokeThickness = 2
            };
            el.Height = el.Width = Radius;
            el.Stroke = Brushes.Black;
            el.Fill = Brushes.Salmon;
            return el;
        }
        private void GetGreedyWay(object sender, EventArgs e)
        {
            if (LeftPoints.Count > 0)
            {

                int CurrentElement = GreedyWay[GreedyWay.Count - 1];
                int MinElement = LeftPoints[0];
                double MinWay = getWay(CurrentElement, MinElement);

                for (int i = 0; i < LeftPoints.Count; i++)
                {
                    double way = getWay(CurrentElement, LeftPoints[i]);
                    if (way < MinWay)
                    {
                        MinWay = way;
                        MinElement = LeftPoints[i];
                    }
                }
                if (LeftPoints.Count != 1)
                    Way[CurrentPoint] = MinElement;
                else
                    Way[CurrentPoint] = LeftPoints[0];
                GreedyWay.Add(MinElement);
                LeftPoints.Remove(MinElement);

                int[] WayWithoutNulls = new int[CurrentPoint + 1];

                for (int i = 0; i < CurrentPoint + 1; i++)
                    WayWithoutNulls[i] = Way[i];

                MyCanvas.Children.Clear();

                PlotPoints();
                PlotWay(WayWithoutNulls);

                Ellipse el = GetEllipse();

                Canvas.SetLeft(el, pC[Way[CurrentPoint]].X - Radius / 2);
                Canvas.SetTop(el, pC[Way[CurrentPoint]].Y - Radius / 2);
                MyCanvas.Children.Add(el);
                CurrentPoint++;
            }
            else
            {
                if (MinimumWay == null)
                    MinimumWay = Way;
                else
               if (GetAllWay(Way) < GetAllWay(MinimumWay))
                    MinimumWay = Way;

                CurrentPointIndex++;
                CurrentPoint = 0;

                if (CurrentPointIndex != PointCount)
                {
                    MyCanvas.Children.Clear();

                    PlotPoints();
                    PlotWay(Way);
                    Thread.Sleep(SleepInterval);
                    GetCurrentPointWay(CurrentPointIndex);
                    MainText.Content = $"Жадібний алгоритм, точка {CurrentPointIndex}";
                }
                else
                {
                    dT2.Stop();
                    StopStart.IsEnabled = true; 
                    NumElemCB.IsEnabled = true;
                    MainText.Content = $"Жадібний алгоритм, найменший шлях";
                    PlotWay(MinimumWay);

                    CurrentPointIndex = 0;
                    Way = null;
                    MinimumWay = null;
                }
            }
        }
        private void GetCurrentPointWay(int index)
        {
            Way = new int[PointCount];

            GreedyWay = new List<int>();
            LeftPoints = CreateNumber(PointCount);

            Way[0] = index;

            GreedyWay.Add(index);
            LeftPoints.Remove(index);

            Ellipse el = GetEllipse();

            MyCanvas.Children.Clear();
            PlotPoints();

            Canvas.SetLeft(el, pC[Way[0]].X - Radius / 2);
            Canvas.SetTop(el, pC[Way[0]].Y - Radius / 2);
            MyCanvas.Children.Add(el);

            CurrentPoint = 1;

        }

        private void NumElemCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            PointCount = Convert.ToInt32(item.Content);
            MakingFirstGeneration();
            InitPoints();
            InitPolygon();
            MyCanvas.Children.Clear();
            PlotPoints();
            CurrentPointIndex = 0;
            Way = null;
            MinimumWay = null;
        }

        private void OneStep(object sender, EventArgs e)
        {
            MyCanvas.Children.Clear();
            PlotPoints();
            PlotWay(GetBestWay());
        }

        private double getWay(int index1, int index2)
        {
            double x1 = pC[index1].X, x2 = pC[index2].X, y1 = pC[index1].Y, y2 = pC[index2].Y;
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }


        private int[] GetBestWay()
        {
            int[] way = new int[PointCount];

            Children = MakingNewGeneration(Parents);
            Parents = FindBestWays(Parents, Children);

            for (int i = 0; i < PointCount; i++)
            {
                way[i] = Parents[0, i];
            }
            return way;
        }

        private void GreedyRegime_Checked(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Clear();
            PlotPoints();

            List<int> hello = CreateNumber(PointCount);
            hello.RemoveAt(0);
            int[] way = new int[PointCount - 1];

            Random rnd = new Random();
            for (int i = 0; i < PointCount - 1; i++)
            {
                int index = rnd.Next(hello.Count);
                way[i] = hello[index];
                hello.RemoveAt(index);
            }
            PlotWay(way);
        }

        private void StopStartGreedy_Click(object sender, RoutedEventArgs e)
        {
            if (dT2.IsEnabled)
            {
                dT2.Stop();
                NumElemCB.IsEnabled = true;
                StopStart.IsEnabled = true;
            }
            else
            {
                NumElemCB.IsEnabled = false;
                StopStart.IsEnabled = false;

                if (Way == null)
                {
                    CurrentPointIndex = 0;
                    GetCurrentPointWay(0);
                }
                MainText.Content = $"Жадібний алгоритм, точка {CurrentPointIndex}";

                dT2.Start();
            }
        }

        private void SleepCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox CB = (ComboBox)e.Source;
            ListBoxItem item = (ListBoxItem)CB.SelectedItem;
            SleepInterval = Convert.ToInt16(item.Content);
        }
    }
}