using System;
using System.Collections.Generic;
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

namespace Paint
{
    public partial class EditElement : Window
    {
        UIElement currentElement;

        public EditElement(UIElement element)
        {
            InitializeComponent();

            currentElement = element;

            if (currentElement.GetType() == typeof(Polyline))
            {
                RunPolylineCase((Polyline)currentElement);
                this.Title = "EditElement - Polyline";
            }
            else if (currentElement.GetType() == typeof(Line))
            {
                RunLineCase((Line)currentElement);
                this.Title = "EditElement - Line";
            }
            else if (currentElement.GetType() == typeof(Ellipse))
            {
                RunEllipseCase((Ellipse)currentElement);
                this.Title = "EditElement - Ellipse";
            }
            else if (currentElement.GetType() == typeof(Rectangle))
            {
                RunRectangleCase((Rectangle)currentElement);
                this.Title = "EditElement - Rectangle";
            }
            else
            {
                MessageBox.Show("Nie znaleziono takiego typu!");
                this.Close();
            }

        }

        private void RunPolylineCase(Polyline polyline)
        {
            ComboList.Text = polyline.StrokeThickness.ToString();
            StrokeButton.Background = polyline.Stroke;
        }

        private void RunLineCase(Line line)
        {
            ComboList.Text = line.StrokeThickness.ToString();
            StrokeButton.Background = line.Stroke;
        }

        private void RunEllipseCase(Ellipse ellipse)
        {
            ComboList.Text = ellipse.StrokeThickness.ToString();
            StrokeButton.Background = ellipse.Stroke;
            FillButton.Background = ellipse.Fill;
        }

        private void RunRectangleCase(Rectangle rectangle)
        {
            ComboList.Text = rectangle.StrokeThickness.ToString();
            StrokeButton.Background = rectangle.Stroke;
            FillButton.Background = rectangle.Fill;
        }


        private void StrokeButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseColorWindow Window = new ChooseColorWindow(StrokeButton.Background);
            Window.Topmost = true;
            Window.ShowDialog();
            if (Window.colorSelected == true)
            {
                StrokeButton.Background = Window.newColor;
                ChangeElementStrokeColor(Window.newColor);
            }
        }

        private void ChangeElementStrokeColor(Brush color)
        {
            if (currentElement.GetType() == typeof(Polyline))
            {
                Polyline polyline = currentElement as Polyline;
                polyline.Stroke = color;
                currentElement = polyline;
            }
            else if (currentElement.GetType() == typeof(Line))
            {
                Line line = currentElement as Line;
                line.Stroke = color;
                currentElement = line;
            }
            else if (currentElement.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = currentElement as Ellipse;
                ellipse.Stroke = color;
                currentElement = ellipse;
            }
            else if (currentElement.GetType() == typeof(Rectangle))
            {
                Rectangle rectangle = currentElement as Rectangle;
                rectangle.Stroke = color;
                currentElement = rectangle;
            }
            else
            {
                MessageBox.Show("Nie znaleziono takiego typu!");
                this.Close();
            }
        }


        private void FillButton_Click(object sender, RoutedEventArgs e)
        {
            ChooseColorWindow Window = new ChooseColorWindow(StrokeButton.Background);
            Window.Topmost = true;
            Window.ShowDialog();
            if (Window.colorSelected == true)
            {
                FillButton.Background = Window.newColor;
                ChangeElementFillColor(Window.newColor);
            }
        }

        private void ChangeElementFillColor(Brush color)
        {
            if (currentElement.GetType() == typeof(Polyline))
            {
                Polyline polyline = currentElement as Polyline;
                polyline.Fill = color;
                currentElement = polyline;
            }
            else if (currentElement.GetType() == typeof(Line))
            {
                Line line = currentElement as Line;
                line.Fill = color;
                currentElement = line;
            }
            else if (currentElement.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = currentElement as Ellipse;
                ellipse.Fill = color;
                currentElement = ellipse;
            }
            else if (currentElement.GetType() == typeof(Rectangle))
            {
                Rectangle rectangle = currentElement as Rectangle;
                rectangle.Fill = color;
                currentElement = rectangle;
            }
            else
            {
                MessageBox.Show("Nie znaleziono takiego typu!");
                this.Close();
            }
        }

        private void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentElement.GetType() == typeof(Polyline))
            {
                Polyline polyline = currentElement as Polyline;
                var item = (ComboBoxItem)ComboList.SelectedValue;
                polyline.StrokeThickness = Double.Parse(item.Content.ToString());
                currentElement = polyline;
            }
            else if (currentElement.GetType() == typeof(Line))
            {
                Line line = currentElement as Line;
                var item = (ComboBoxItem)ComboList.SelectedValue;
                line.StrokeThickness = Double.Parse(item.Content.ToString());
                currentElement = line;
            }
            else if (currentElement.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = currentElement as Ellipse;
                var item = (ComboBoxItem)ComboList.SelectedValue;
                ellipse.StrokeThickness = Double.Parse(item.Content.ToString());
                currentElement = ellipse;
            }
            else if (currentElement.GetType() == typeof(Rectangle))
            {
                Rectangle rectangle = currentElement as Rectangle;
                var item = (ComboBoxItem)ComboList.SelectedValue;
                rectangle.StrokeThickness = Double.Parse(item.Content.ToString());
                currentElement = rectangle;
            }
            else
            {
                MessageBox.Show("Nie znaleziono takiego typu!");
                this.Close();
            }
        }
    }
}
