using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;

namespace Paint
{
    public partial class MainWindow : Window
    {
        private enum MyTools
        {
            Brush, Line, Ellipse, Rectangle,
        }
 
        private MyTools currentTool;

        private Brush firstColor;
        private Brush secondColor;
        private Brush currentColor;

        private UndoRedoControl UndoRedo;
        List<Point> brushPoints;  //pedzel tworzy zbior elementów (punktów)
        List<UIElement> brushLines;  //pedzel zapisywany jest jako polyline po zakonczeniu, ale chcemy widziec go też w czasie rzeczywistym

        bool isEditing;
        UIElement currentEditElement = null;

        private double ToolbarHeight;
        Point start;
        Point end;


        public MainWindow()
        {
            InitializeComponent();

            firstColor = Brushes.Black;
            FirstColorButton.Background = firstColor;

            secondColor = Brushes.White;
            SecondColorButton.Background = secondColor;

            ToolbarHeight = 100;

            currentTool = MyTools.Brush;
            UndoRedo = new UndoRedoControl();
            brushPoints = new List<Point>();
            brushLines = new List<UIElement>();

            isEditing = false;

        }

        private void BrushButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = MyTools.Brush;
        }
        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = MyTools.Line;
        }
 
        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = MyTools.Ellipse;
        }
 
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            currentTool = MyTools.Rectangle;
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isEditing)
            {
                EditElement();
            }
            else
            {
                start = e.GetPosition(this);

                if (e.LeftButton == MouseButtonState.Pressed) // sprawdzanie czy lewy, czy prawy przycisk myszki
                {
                    currentColor = firstColor;
                }
                else
                {
                    currentColor = secondColor;
                }
            }
        }
 
        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isEditing == false)
            {
                switch (currentTool)
                {
                    case MyTools.Brush:
                        brushLines.Clear();
                        DrawPolyline();
                        break;

                    case MyTools.Line:
                        DrawLine();

                        break;

                    case MyTools.Ellipse:
                        DrawEllipse();
                        break;

                    case MyTools.Rectangle:
                        DrawRectangle();
                        break;
                }
            }
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            if ((e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed) && isEditing == false )
            {
                end = e.GetPosition(this);

                if (currentTool == MyTools.Brush)
                {
                    Polyline polyline = new Polyline();

                    polyline.Stroke = currentColor;
                    polyline.StrokeThickness = 4;

                    Point currentPoint = new Point();

                    currentPoint.X = e.GetPosition(this).X;
                    currentPoint.Y = e.GetPosition(this).Y - ToolbarHeight;

                    brushPoints.Add(currentPoint);
                    // do tego momentu zbieranie punktów do polyline, potem rysowanie w czasie rzeczywistym


                    Line line = new Line();

                    if (e.LeftButton == MouseButtonState.Pressed )
                    {
                        line.Stroke = firstColor;
                    }
                    else
                    {
                        line.Stroke = secondColor;
                    }

                    line.StrokeThickness = 4;
                    line.X1 = start.X;
                    line.Y1 = start.Y - ToolbarHeight;
                    line.X2 = e.GetPosition(this).X;
                    line.Y2 = e.GetPosition(this).Y - ToolbarHeight;

                    start = e.GetPosition(this);

                    MyCanvas.Children.Add(line);
                    brushLines.Add(line);
                }
            }
        }

        private void DrawPolyline()
        {
            Polyline polyline = new Polyline()
            {
                Stroke = currentColor,
                StrokeThickness = 4,
            };

            foreach (Point point in brushPoints)
            {
                polyline.Points.Add(point);
            }
            brushPoints.Clear();

            MyCanvas.Children.Add(polyline);
            UndoRedo.AddNormalLog(polyline, "Polyline");
        }

        private void DrawLine()
        {
            Line line = new Line()
            {
                Stroke = currentColor,
                StrokeThickness = 4,
                X1 = start.X,
                Y1 = start.Y - ToolbarHeight,
                X2 = end.X,
                Y2 = end.Y - ToolbarHeight
            };
            MyCanvas.Children.Add(line);
            UndoRedo.AddNormalLog(line,"Line");
        }
 
        private void DrawEllipse()
        {
            Ellipse newEllipse = new Ellipse()
            {
                StrokeThickness = 4,
            };

            if (currentColor == firstColor) // kontur i wypelnienie, pracy czy lewy przycisk myszki
            {
                newEllipse.Stroke = firstColor;
                newEllipse.Fill = secondColor;
            }
            else
            {
                newEllipse.Stroke = secondColor;
                newEllipse.Fill = firstColor;
            }

            if (end.X >= start.X)
            {
                newEllipse.SetValue(Canvas.LeftProperty, start.X);
                newEllipse.Width = end.X - start.X;
            }
            else
            {
                newEllipse.SetValue(Canvas.LeftProperty, end.X);
                newEllipse.Width = start.X - end.X;
            }
 
            if (end.Y >= start.Y)
            {
                newEllipse.SetValue(Canvas.TopProperty, start.Y - ToolbarHeight);
                newEllipse.Height = end.Y - start.Y;
            }
            else
            {
                newEllipse.SetValue(Canvas.TopProperty, end.Y - ToolbarHeight);
                newEllipse.Height = start.Y - end.Y;
            }
 
            MyCanvas.Children.Add(newEllipse);
            UndoRedo.AddNormalLog(newEllipse, "Ellipse");
        }
 
        private void DrawRectangle()
        {
            Rectangle newRectangle = new Rectangle()
            {
                StrokeThickness = 4
            };

            if (currentColor == firstColor) // kontur i wypelnienie, pracy czy lewy przycisk myszki
            {
                newRectangle.Stroke = firstColor;
                newRectangle.Fill = secondColor;
            }
            else
            {
                newRectangle.Stroke = secondColor;
                newRectangle.Fill = firstColor;
            }

            if (end.X >= start.X)
            {
                newRectangle.SetValue(Canvas.LeftProperty, start.X);
                newRectangle.Width = end.X - start.X;
            }
            else
            {
                newRectangle.SetValue(Canvas.LeftProperty, end.X);
                newRectangle.Width = start.X - end.X;
            }
 
            if (end.Y >= start.Y)
            {
                newRectangle.SetValue(Canvas.TopProperty, start.Y - ToolbarHeight);
                newRectangle.Height = end.Y - start.Y;
            }
            else
            {
                newRectangle.SetValue(Canvas.TopProperty, end.Y - ToolbarHeight);
                newRectangle.Height = start.Y - end.Y;
            }
 
            MyCanvas.Children.Add(newRectangle);
            UndoRedo.AddNormalLog(newRectangle, "Rectangle");
        }

        public static Color ConvertCmykToRgb(float c, float m, float y, float k)
        {
            int r;
            int g;
            int b;

            r = Convert.ToInt32(255 * (1 - c) * (1 - k));
            g = Convert.ToInt32(255 * (1 - m) * (1 - k));
            b = Convert.ToInt32(255 * (1 - y) * (1 - k));

            return Color.FromArgb((byte)k, (byte)r, (byte)g, (byte)b);
        }

        public static CmykColor ConvertRgbToCmyk(int r, int g, int b)
        {
            float c;
            float m;
            float y;
            float k;
            float rf;
            float gf;
            float bf;

            rf = r / 255F;
            gf = g / 255F;
            bf = b / 255F;

            k = ClampCmyk(1 - Math.Max(Math.Max(rf, gf), bf));
            c = ClampCmyk((1 - rf - k) / (1 - k));
            m = ClampCmyk((1 - gf - k) / (1 - k));
            y = ClampCmyk((1 - bf - k) / (1 - k));

            return new CmykColor(c, m, y, k);
        }

        private static float ClampCmyk(float value)
        {
            if (value < 0 || float.IsNaN(value))
            {
                value = 0;
            }

            return value;
        }

        private void PaletteColorsLeft_Click(object sender, RoutedEventArgs e)
        {
            firstColor = ((Button)sender).Background;
            FirstColorButton.Background = firstColor;
        }

        private void PaletteColorsRight_Click(object sender, RoutedEventArgs e)
        {
            secondColor = ((Button)sender).Background;
            SecondColorButton.Background = secondColor;
        }

        private void FirstColorButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseColorWindow Window = new ChooseColorWindow(firstColor);
            Window.Topmost = true;
            Window.ShowDialog();
            if (Window.colorSelected == true)
            {
                firstColor = Window.newColor;
                FirstColorButton.Background = firstColor;
            }
        }

        private void ToolbarScrollRemover_Loaded(object sender, RoutedEventArgs e)
        {
                ToolBar toolBar = sender as ToolBar;
                var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
                if (overflowGrid != null)
                {
                    overflowGrid.Visibility = Visibility.Collapsed;
                }

                var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
                if (mainPanelBorder != null)
                {
                    mainPanelBorder.Margin = new Thickness(0);
                }
        }

        private void SecondColorButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseColorWindow Window = new ChooseColorWindow(secondColor);
            Window.Topmost = true;
            Window.ShowDialog();

            if (Window.colorSelected == true)
            {
                secondColor = Window.newColor;
                SecondColorButton.Background = secondColor;
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Clear();

            foreach (var item in UndoRedo.Undo())
            {
                MyCanvas.Children.Add(item);
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            MyCanvas.Children.Clear();

            foreach (var item in UndoRedo.Redo())
            {
                MyCanvas.Children.Add(item);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing == false)
            {
                isEditing = true;
                EditButton.Content = "Edit mode: ON";
            }
            else
            {
                isEditing = false;
                EditButton.Content = "Edit mode: OFF";
            }
        }

        private void EditElement()
        {
            foreach (UIElement item in MyCanvas.Children)
            {
                if (item.IsMouseDirectlyOver)
                {
                    currentEditElement = item;
                }
            }

            if (currentEditElement != null) //czy znaleziono element
            {
                EditElement window = new EditElement(currentEditElement);
                window.ShowDialog();
            }

            currentEditElement = null;
        }

        private void FiltersButton_Click(object sender, RoutedEventArgs e)
        {
                FiltersWindow window = new FiltersWindow(MyCanvas);
                window.ShowDialog();
        }
    }
}