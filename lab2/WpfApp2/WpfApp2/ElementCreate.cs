using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    class ECreate
    {
        static public Window WNCreate(double width, double height, string title)
        {
            Window WN = new Window
            {
                Width = width,
                Height = height,
                ResizeMode = ResizeMode.NoResize,
                Title = title,
                Background = Brushes.LightSkyBlue
            };
            return WN;
        }
        static public Grid GridCreate(double width, double height)
        {
            Grid LayoutRoot = new Grid
            {
                Width = width,
                Height = height,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true,
            };
            return LayoutRoot;
        }
        static public Button ButtonCreate(double h, double w, double m_top,
           double m_left, string content, SolidColorBrush b, Grid LayoutRoot)
        {
            Button butt = new Button
            {
                Content = content,
                Width = w,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(m_left, m_top, 0, 0),
                Height = h,
                FontSize = 16,
               FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI"),
                Background = b,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            LayoutRoot.Children.Add(butt);
            return butt;
        }
        static public Button ButtonCreate(double h, double w, double m_top,
           double m_left,string name, string content, SolidColorBrush b, Grid LayoutRoot)
        {
            Button butt = new Button
            {
                Content = content,
                Width = w,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(m_left, m_top, 0, 0),
                Height = h,
                FontSize = 16,
               FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI"),
                Background = b,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            LayoutRoot.Children.Add(butt);
            return butt;
        }
        static public TextBox CreateTBox(double width, double height, double left, double top, double size, FontFamily font, Grid LayoutRoot)
        {
            TextBox tb = new TextBox
            {
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(left, top, 0, 0),
                Text = "",
                TextWrapping = TextWrapping.Wrap,
                Width = width,
                Height = height,
                Background = Brushes.Beige,
                FontSize = size,
                FontFamily = font
            };
            LayoutRoot.Children.Add(tb);
            return tb;
        }
        static public TextBlock CreateTBlock(double width, double height,double left, double top,double size, string content,
            FontFamily font, Grid LayoutRoot)
        {
            TextBlock tb = new TextBlock
            {
                TextAlignment = TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(left, top, 0, 0),
                Text = content,
                TextWrapping = TextWrapping.Wrap,
                Width = width,
                Height = height,
                FontSize = size,
                FontFamily = font,
                FontWeight = FontWeights.Bold
            };
            LayoutRoot.Children.Add(tb);
            return tb;
        }
        static public TextBlock CreateTBlock(double width, double height,double left, double top,double size, string content,
            FontFamily font)
        {
            TextBlock tb = new TextBlock
            {
                TextAlignment = TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(left, top, 0, 0),
                Text = content,
                TextWrapping = TextWrapping.Wrap,
                Width = width,
                Height = height,
                FontSize = size,
                FontFamily = font,
                FontWeight = FontWeights.Bold
            };
            return tb;
        }

    }
}
