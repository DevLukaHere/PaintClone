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
    public partial class ChooseColorWindow : Window
    {
        public bool _updating { get; private set; }

        public ChooseColorWindow(Brush currentColor)
        {
            InitializeComponent();
            newColor = currentColor;
            HexLabel.Text = HexValue(currentColor.ToString());
            LoadColor();
        }

        public Brush newColor;
        public bool colorSelected = false;

        private void LoadColor()
        {
            Color color = ((SolidColorBrush)newColor).Color;

            if (!_updating)
            {
                _updating = true;

                RedSlider.Value = color.R;
                GreenSlider.Value = color.G;
                BlueSlider.Value = color.B;

                _updating = false;
            }
            ConvertFromRgb();
        }

        private void CyanSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CyanPercent.Content = (int)CyanSlider.Value + "%";
            ConvertFromCmyk();
            ChangeCmykLabel();
        }

        private void MagentaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MagentaPercent.Content = (int)MagentaSlider.Value + "%";
            ConvertFromCmyk();
            ChangeCmykLabel();
        }

        private void YellowSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            YellowPercent.Content = (int)YellowSlider.Value + "%";
            ConvertFromCmyk();
            ChangeCmykLabel();
        }

        private void BlackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BlackPercent.Content = (int)BlackSlider.Value + "%";
            ConvertFromCmyk();
            ChangeCmykLabel();
        }


        private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RedPercent.Content = (int)RedSlider.Value;
            ConvertFromRgb();
            ChangeRgbLabel();
        }

        private void GreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GreenPercent.Content = (int)GreenSlider.Value;
            ConvertFromRgb();
            ChangeRgbLabel();
        }

        private void BlueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BluePercent.Content = (int)BlueSlider.Value;
            ConvertFromRgb();
            ChangeRgbLabel();
        }

        private void ChangeCmykLabel()
        {
            CmykLabel.Content = "CMYK: " + (int)CyanSlider.Value + "," + (int)MagentaSlider.Value + "," + (int)YellowSlider.Value + "," + (int)BlackSlider.Value;
        }

        private void ChangeRgbLabel()
        {
            RGBLabel.Content = "RGB: " + (int)RedSlider.Value + "," + (int)GreenSlider.Value + "," + (int)BlueSlider.Value;
        }

        private void ConvertFromRgb()
        {
            int r;
            int g;
            int b;
            CmykColor converted;

            r = (int)RedSlider.Value;
            g = (int)GreenSlider.Value;
            b = (int)BlueSlider.Value;

            SolidColorBrush myColor = new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
            newColor = myColor;
            CurrentColor.Background = myColor;
            HexLabel.Text = HexValue(myColor.ToString());

            converted = ColorConvert.ConvertRgbToCmyk(r, g, b);

            if (!_updating)
            {
                _updating = true;

                CyanSlider.Value = Convert.ToInt32(converted.C * 100);
                MagentaSlider.Value = Convert.ToInt32(converted.M * 100);
                YellowSlider.Value = Convert.ToInt32(converted.Y * 100);
                BlackSlider.Value = Convert.ToInt32(converted.K * 100);

                _updating = false;
            }
        }

        private void ConvertFromCmyk()
        {
            float c;
            float m;
            float y;
            float k;
            Color converted;

            c = (float)(CyanSlider.Value / 100);
            m = (float)(MagentaSlider.Value / 100);
            y = (float)(YellowSlider.Value / 100);
            k = (float)(BlackSlider.Value / 100);

            converted = ColorConvert.ConvertCmykToRgb(c, m, y, k);

            if (!_updating)
            {
                _updating = true;

                RedSlider.Value = converted.R;
                GreenSlider.Value = converted.G;
                BlueSlider.Value = converted.B;

                _updating = false;
            }
        }

        private string HexValue(string value)
        {
            string removed = value.Remove(1, 2);
            return removed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            colorSelected = true;
            this.Close();
        }
    }
}
