using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace Paint
{
    public class FilterMe
    {
        public FilterMe()
        {
           
        }

        public Bitmap Start(Bitmap myBitmap, int[,] mask)
        {
            if (myBitmap == null)
            {
                System.Windows.Forms.MessageBox.Show("Pusty bitmap!");
            }

            Bitmap newBitmap = new Bitmap(myBitmap.Width, myBitmap.Height);
            Point point;

            for (int j = 1; j < myBitmap.Height - 1; j++)
            {
                for (int i = 1; i < myBitmap.Width - 1; i++)
                {

                    point = new Point(i, j); //dla danego punktu

                    int maskSum = CalculateMaskSum(mask); //suma maski
                    if(maskSum == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Suma maski jest równa 0!");
                        return null;
                    }

                    int elementK = CalculateWeightedSum(mask, myBitmap, point, 'k') / maskSum;
                    int elementR = CalculateWeightedSum(mask, myBitmap, point, 'r') / maskSum;
                    int elementG = CalculateWeightedSum(mask, myBitmap, point, 'g') / maskSum;
                    int elementB = CalculateWeightedSum(mask, myBitmap, point, 'b') / maskSum; //liczenie sumy wazonej dla kazdej skladowej koloru

                    elementK = Normalize(elementK);
                    elementR = Normalize(elementR);
                    elementG = Normalize(elementG);
                    elementB = Normalize(elementB); //jezeli mniej niz 0, lub wieksze niz 255

                    Color newColor = new Color();
                    newColor = Color.FromArgb((byte)elementK, (byte)elementR, (byte)elementG, (byte)elementB);

                    newBitmap.SetPixel(point.X, point.Y, newColor);

                }
            }

            return newBitmap;
        }

        private int Normalize(int element)
        {
            int newValue = element;

            if (element < 0)
            {
                newValue = 0;
            }

            if (element > 255)
            {
                newValue = 255;
            }

            return newValue;
        }

        private int CalculateWeightedSum(int[,] mask, Bitmap bitmap, Point point, char whichElement)
        {
            int[] values = new int[9];

            if (whichElement == 'k')
            {
                values[0] = bitmap.GetPixel(point.X - 1, point.Y - 1).A * mask[0, 0];
                values[1] = bitmap.GetPixel(point.X - 0, point.Y - 1).A * mask[0, 1];
                values[2] = bitmap.GetPixel(point.X + 1, point.Y - 1).A * mask[0, 2];
                values[3] = bitmap.GetPixel(point.X - 1, point.Y - 0).A * mask[1, 0];
                values[4] = bitmap.GetPixel(point.X - 0, point.Y - 0).A * mask[1, 1];
                values[5] = bitmap.GetPixel(point.X + 1, point.Y - 0).A * mask[1, 2];
                values[6] = bitmap.GetPixel(point.X - 1, point.Y + 1).A * mask[2, 0];
                values[7] = bitmap.GetPixel(point.X - 0, point.Y + 1).A * mask[2, 1];
                values[8] = bitmap.GetPixel(point.X + 1, point.Y + 1).A * mask[2, 2];
            }
            else if (whichElement == 'r')
            {
                values[0] = bitmap.GetPixel(point.X - 1, point.Y - 1).R * mask[0, 0];
                values[1] = bitmap.GetPixel(point.X - 0, point.Y - 1).R * mask[0, 1];
                values[2] = bitmap.GetPixel(point.X + 1, point.Y - 1).R * mask[0, 2];
                values[3] = bitmap.GetPixel(point.X - 1, point.Y - 0).R * mask[1, 0];
                values[4] = bitmap.GetPixel(point.X - 0, point.Y - 0).R * mask[1, 1];
                values[5] = bitmap.GetPixel(point.X + 1, point.Y - 0).R * mask[1, 2];
                values[6] = bitmap.GetPixel(point.X - 1, point.Y + 1).R * mask[2, 0];
                values[7] = bitmap.GetPixel(point.X - 0, point.Y + 1).R * mask[2, 1];
                values[8] = bitmap.GetPixel(point.X + 1, point.Y + 1).R * mask[2, 2];
            }
            else if (whichElement == 'g')
            {
                values[0] = bitmap.GetPixel(point.X - 1, point.Y - 1).G * mask[0, 0];
                values[1] = bitmap.GetPixel(point.X - 0, point.Y - 1).G * mask[0, 1];
                values[2] = bitmap.GetPixel(point.X + 1, point.Y - 1).G * mask[0, 2];
                values[3] = bitmap.GetPixel(point.X - 1, point.Y - 0).G * mask[1, 0];
                values[4] = bitmap.GetPixel(point.X - 0, point.Y - 0).G * mask[1, 1];
                values[5] = bitmap.GetPixel(point.X + 1, point.Y - 0).G * mask[1, 2];
                values[6] = bitmap.GetPixel(point.X - 1, point.Y + 1).G * mask[2, 0];
                values[7] = bitmap.GetPixel(point.X - 0, point.Y + 1).G * mask[2, 1];
                values[8] = bitmap.GetPixel(point.X + 1, point.Y + 1).G * mask[2, 2];
            }
            else if (whichElement == 'b')
            {
                values[0] = bitmap.GetPixel(point.X - 1, point.Y - 1).B * mask[0, 0];
                values[1] = bitmap.GetPixel(point.X - 0, point.Y - 1).B * mask[0, 1];
                values[2] = bitmap.GetPixel(point.X + 1, point.Y - 1).B * mask[0, 2];
                values[3] = bitmap.GetPixel(point.X - 1, point.Y - 0).B * mask[1, 0];
                values[4] = bitmap.GetPixel(point.X - 0, point.Y - 0).B * mask[1, 1];
                values[5] = bitmap.GetPixel(point.X + 1, point.Y - 0).B * mask[1, 2];
                values[6] = bitmap.GetPixel(point.X - 1, point.Y + 1).B * mask[2, 0];
                values[7] = bitmap.GetPixel(point.X - 0, point.Y + 1).B * mask[2, 1];
                values[8] = bitmap.GetPixel(point.X + 1, point.Y + 1).B * mask[2, 2];
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Zly element!");
            }

            int sum = 0;
            foreach (int item in values)
            {
                sum += item;
            }

            return sum;
        }

        private int CalculateMaskSum(int[,] mask)
        {
            int maskSum = 0;
            foreach (int item in mask)
            {
                maskSum += item;
            }
            return maskSum;
        }
    }
}
