using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UMD.Canvas
{
    /// <summary>
    /// Interaction logic for Canvas.xaml
    /// </summary>
    public partial class Canvas : UserControl
    {
        public static readonly DependencyProperty BackgroundImageProperty =
        DependencyProperty.Register(
            nameof(BackgroundImage),
            typeof(ImageSource),
            typeof(Canvas),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnBackgroundImageChanged))
        );

        private double Scale = 1;

        private double MaxScale = 10;
        private double MinScale = 0.5;

        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set
            {
                SetValue(BackgroundImageProperty, value);
            }
        }

        private static void OnBackgroundImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Canvas ctrl = obj as Canvas;
            if (ctrl != null)
            {
                ImageBrush background = new ImageBrush(ctrl.BackgroundImage);
                ctrl.Map.Background = background;
                ctrl.Map.Width = ctrl.BackgroundImage.Width;
                ctrl.Map.Height = ctrl.BackgroundImage.Height;
            }
        }

        public Canvas()
        {
            InitializeComponent();
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            double scale = Math.Max(e.DeltaManipulation.Scale.X, e.DeltaManipulation.Scale.Y);

            if (scale == 1)
            {
                MoveImage(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
            }
            else if (scale != 0)
            {
                Scale *= scale;

                if (MinScale < Scale && Scale < MaxScale)
                {
                    MoveImage(Map.ActualWidth * (1 - scale) / 2, Map.ActualHeight * (1 - scale) / 2);
                    Map.LayoutTransform = new ScaleTransform(Scale, Scale);
                }
            }
        }

        private void MoveImage(double x, double y)
        {
            Map.Margin = new Thickness(
                Map.Margin.Left + x,
                Map.Margin.Top + y, 0, 0);
        }

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
