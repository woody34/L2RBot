using Managed.Adb;
using System.Drawing;

namespace L2RBot
{
    public class Pixel
    {
        private Screen ScreenObj;

        public Color Color { get; set; }

        public Point Point { get; set; }


        /// <summary>
        /// Looks for the Color at the Point on the game screen.
        /// </summary>
        /// <param name="Screen"></param>
        /// <param name="Tolerance"></param>
        /// <returns>True: Color detected</returns>
        /// <returns>False: Color NOT detected</returns>
        public bool IsPresent(Rectangle Screen, int Tolerance)
        {
            ScreenObj = new Screen();

            return L2RBot.Screen.CompareColor(Color, ScreenObj.GetColor(Screen, Point.X, Point.Y), Tolerance);
        }

        public bool IsPresent(Image Image, int Tolerance)
        {
            Bitmap ScreenCap = new Bitmap(Image);

            return L2RBot.Screen.CompareColor(Color, ScreenCap.GetPixel(Point.X, Point.Y), Tolerance);
        }

        /// <summary>
        /// Grabs game screen's current Color value at Point.
        /// </summary>
        /// <param name="Screen"></param>
        /// <returns></returns>
        public Color CurrentValue(Rectangle Screen)
        {
            ScreenObj = new Screen();

            return ScreenObj.GetColor(Screen, Point.X, Point.Y);
        }

        public Color CurrentValue(Image Image)
        {
            Bitmap ScreenCap = new Bitmap(Image);

            return ScreenCap.GetPixel(Point.X, Point.Y);
        }

        /// <summary>
        /// Updates the Pixel's Color value with the current game screen Color at the defined Point.
        /// </summary>
        /// <param name="Screen"></param>
        public void UpdateColor(Rectangle Screen)
        {
            Color = CurrentValue(Screen);
        }

        public void UpdateColor(Image Image)
        {
            Bitmap ScreenCap = new Bitmap(Image);

            Color = ScreenCap.GetPixel(Point.X, Point.Y);
        }

        /// <summary>
        /// Displays Pixel's string value in a specific format.
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            return "(" + Point.X + ", " + Point.Y + ")Target[" + Color.A + "," + Color.R + "," + Color.G + "," + Color.B + "]";
        }
    }
}
