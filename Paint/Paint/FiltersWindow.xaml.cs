using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Interop;
using Point = System.Drawing.Point;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace Paint
{

    public partial class FiltersWindow : Window
    {
        Bitmap myBitmap;
        Bitmap newBitmap;
        FilterMe filterMe;

        int[,] lowmask = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
        int[,] highmask = { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } };
        int[,] laplacemask = { { -1, -1, -1 }, { -1, 8, 1 }, { -1, -1, -1 } };

        public FiltersWindow(Canvas mycanvas)
        {
            InitializeComponent();
            ExportCanvasAsBitmap(mycanvas);
            LeftImage.Source = ImageSourceFromBitmap(myBitmap);
            filterMe = new FilterMe();
            
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button myButton = sender as Button;

            switch (myButton.Content.ToString())
            {
                case "Low-pass":
                    Mask1.Text = lowmask[0, 0].ToString();
                    Mask2.Text = lowmask[0, 1].ToString();
                    Mask3.Text = lowmask[0, 2].ToString();

                    Mask4.Text = lowmask[1, 0].ToString();
                    Mask5.Text = lowmask[1, 1].ToString();
                    Mask6.Text = lowmask[1, 2].ToString();

                    Mask7.Text = lowmask[2, 0].ToString();
                    Mask8.Text = lowmask[2, 1].ToString();
                    Mask9.Text = lowmask[2, 2].ToString();
                    break;

                case "High-pass":
                    Mask1.Text = highmask[0, 0].ToString();
                    Mask2.Text = highmask[0, 1].ToString();
                    Mask3.Text = highmask[0, 2].ToString();

                    Mask4.Text = highmask[1, 0].ToString();
                    Mask5.Text = highmask[1, 1].ToString();
                    Mask6.Text = highmask[1, 2].ToString();

                    Mask7.Text = highmask[2, 0].ToString();
                    Mask8.Text = highmask[2, 1].ToString();
                    Mask9.Text = highmask[2, 2].ToString();
                    break;
                case "Laplace":
                    Mask1.Text = laplacemask[0, 0].ToString();
                    Mask2.Text = laplacemask[0, 1].ToString();
                    Mask3.Text = laplacemask[0, 2].ToString();

                    Mask4.Text = laplacemask[1, 0].ToString();
                    Mask5.Text = laplacemask[1, 1].ToString();
                    Mask6.Text = laplacemask[1, 2].ToString();

                    Mask7.Text = laplacemask[2, 0].ToString();
                    Mask8.Text = laplacemask[2, 1].ToString();
                    Mask9.Text = laplacemask[2, 2].ToString();
                    break;
                default:
                    break;
            }

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\Images\\slontester.png";
            myBitmap = new Bitmap(filePath);
            LeftImage.Source = ImageSourceFromBitmap(myBitmap);
        }


        public void ExportCanvasAsBitmap(Canvas surface)
        {
            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Get the size of canvas
            System.Windows.Size size = new System.Windows.Size(surface.ActualWidth, surface.ActualHeight);
            // Measure and arrange the surface
            // VERY IMPORTANT
            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);


            myBitmap = new Bitmap(stream);


            surface.LayoutTransform = transform;
        }

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumber(Mask1.Text) && IsNumber(Mask2.Text) && IsNumber(Mask3.Text) && IsNumber(Mask4.Text) && IsNumber(Mask5.Text) && IsNumber(Mask6.Text) && IsNumber(Mask7.Text) && IsNumber(Mask8.Text) && IsNumber(Mask9.Text))
            {
                int[,] mask = {
                { int.Parse(Mask1.Text), int.Parse(Mask2.Text), int.Parse(Mask3.Text) },
                { int.Parse(Mask4.Text), int.Parse(Mask5.Text), int.Parse(Mask6.Text) },
                { int.Parse(Mask7.Text), int.Parse(Mask8.Text), int.Parse(Mask9.Text) }
            };
                newBitmap = filterMe.Start(myBitmap, mask);
                if (newBitmap != null)
                {
                    RightImage.Source = ImageSourceFromBitmap(newBitmap);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Błędna maska!");
            }
        }

        private bool IsNumber (string text)
        {
            int number;
            if (Int32.TryParse(text, out number))
                return true;
            else
                return false;
        }
    }
}
