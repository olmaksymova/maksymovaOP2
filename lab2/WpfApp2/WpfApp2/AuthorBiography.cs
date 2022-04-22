using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    class AuthorBiography
    {
        public AuthorBiography()
        {
            InitMe();
        }
        Window wn;
        Grid LayoutRoot;
        private void InitMe()
        {
            wn = ECreate.WNCreate(450, 220, "Про автора");
            LayoutRoot = ECreate.GridCreate(wn.Width, wn.Height);

            TextBlock MyTitle = ECreate.CreateTBlock(wn.Width, 45, 0, 20, 22, "Про автора", new FontFamily("Arial Black"), LayoutRoot);
            MyTitle.TextAlignment = TextAlignment.Center;

            TextBlock MyText = ECreate.CreateTBlock(wn.Width - 30, 150, 30, 60, 20,
                "ПІБ: Максимова Ольга Валеріївна \nНазва роботи: Лабораторна робота з ОП\nРік створення: 2022",
                new FontFamily("Segoe UI"), LayoutRoot);
            Button ToMainWindow = ECreate.ButtonCreate(30, 100, 150, 80, "Назад", Brushes.Salmon, LayoutRoot);
            Button Exit = ECreate.ButtonCreate(30, 100, 150, 250, "Вийти", Brushes.Salmon, LayoutRoot);

            Exit.Click += Exit_Click;
            ToMainWindow.Click += ToMainWindow_Click;

            wn.Content = LayoutRoot;
            wn.Show();
        }
        private void ToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            wn.Hide();
            MainWindow MW = new();
            MW.Show();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}