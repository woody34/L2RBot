using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;




namespace L2RBot
{
    class Screen
    {

        public static int topBoarder = 30;
        public static int bottomBorder = 2;
        public static int leftBorder = 2;
        public static int rightBorder = 2;
        public static int boarderWidth = leftBorder + rightBorder;
        public static int boarderHeight = topBoarder + bottomBorder;

        public static Rectangle GetRect(Process proc)
        {
            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            Rectangle rectangle = new Rectangle(rect.left, rect.top, width, height);
            if (rectangle.Width != (1280 + rightBorder + leftBorder) | rectangle.Height != (720 + topBoarder + bottomBorder))
            {
                System.Windows.MessageBox.Show(proc.MainWindowTitle + " needs reset to 1280x720 or Nox player left and top borders have changed. Did you update Nox Player?");
            }
            return rectangle;
        }
        public static Point PercentToPoint(Rectangle rect,double X, double Y)
        {
            int mWidth = rect.Width - boarderWidth;
            double mWidthD = (double)mWidth / 100;
            double dWidth = (double)mWidthD * X;

            int mHeight = rect.Height - boarderHeight;
            double mHeightD = (double)mHeight / 100;
            double dHeight = (double)mHeightD * Y;

            int x = (int)Math.Round(dWidth);
            int y = (int)Math.Round(dHeight);
            Point cClick = new Point(x, y); //testing pourposes

            x += rect.X + leftBorder;
            y += rect.Y + topBoarder;

            return new Point(x, y);

        }
        public static Point PointToScreenPoint(Rectangle rect, int X, int Y)
        {
            int x;
            int y;
            x = rect.X + X + leftBorder;
            y = rect.Y + Y + topBoarder;
            //Debug.WriteLine(X + "," + Y + "|"+ x + "," + y);
            return new Point(x, y);

        }
        public static Rectangle PercentToRect(Rectangle rect, double X, double Y, double Width, double Height)
        { 
            
            Point point = PercentToPoint(rect, X, Y);
            Point dem = PercentToPoint(rect, Width, Height);
            Rectangle inner = new Rectangle(point.X, point.Y, dem.X, dem.Y);

            return inner;
        }
        public Bitmap CaptureApplication(Process proc)
        {

            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            bmp.Save(@"test.PNG", ImageFormat.Png);
            return bmp;
        }
        public Bitmap CaptureApplication(Process proc, out Rectangle rectangle)
        {

            var rect = new User32.Rect();
            //Console.WriteLine(proc.MainWindowTitle + proc.MainWindowHandle.ToString());
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;
            //Console.WriteLine(rect.right.ToString() + rect.left.ToString() + rect.bottom.ToString() + rect.top.ToString());
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            //point = new Point(rect.left, rect.top);
            rectangle = new Rectangle(rect.left, rect.top, width, height);

            return bmp;
        }
        public static Color GetColor(Rectangle rect, int X, int Y)
        {
            Point point = PointToScreenPoint(rect, X, Y);
            int x = point.X;
            int y = point.Y;
            //Debug.WriteLine("GetColor");
            IntPtr desk = User32.GetDesktopWindow();
            IntPtr dc = User32.GetWindowDC(desk);
            int a = (int)User32.GetPixel(dc, x, y);
            User32.ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
        /// <summary>
        /// This method takes the percentage x and y position values that are created from a Nox Player screenshot
        /// It receives the windows position and size data in the form of a rectangle
        /// Tt will subtract the 2,6,2,2 pixel boarders from the x and y values before calculating pixel location(since they are being determined with a screenshot that has no boarders.
        /// It maths the Perctage values to represent the actual screen locations of the pixels to account for Nox Player screen resizing or movement of the Nox Player Window
        /// </summary>
        /// <param name="rect">Windows postistion and dementions represented in a Rectangle</param>
        /// <param name="X">Percentage of the screen's x value devised from a nox screenshot(takes into account the 2 boarders and maths them away)</param>
        /// <param name="Y">Percentage of the screen's y value devised from a nox screenshot(takes into account the 2 boarders and maths them away)</param>
        /// <returns></returns>
        public static Color GetColor(Rectangle rect, double X, double Y)//Input control click % for varrious screen sizes
        {
            Point point = PercentToPoint(rect, X, Y);
            int x = point.X;
            int y = point.Y;

            IntPtr desk = User32.GetDesktopWindow();
            IntPtr dc = User32.GetWindowDC(desk);
            int a = (int)User32.GetPixel(dc, x, y);
            User32.ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
        public static Color GetColor(Rectangle rect, double X, double Y, out Point click_location)
        {
            Point point = PercentToPoint(rect, X, Y);
            int x = point.X;
            int y = point.Y;

            click_location = new Point(x, y);

            IntPtr desk = User32.GetDesktopWindow();
            IntPtr dc = User32.GetWindowDC(desk);
            int a = (int)User32.GetPixel(dc, x, y);
            User32.ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
        public static Boolean ComparePixel(Color Pixel, Color Find, int Tolerance)
        {
            //compare pixel exact values
            if (Pixel.R == Find.R & Pixel.G == Find.G & Pixel.B == Find.B)
            {
                return true;
            }
            //for each value in tolerance, compare pixels
            for(int i = 0; i <= Tolerance; i++)
            {
                if (Pixel.R + i == Find.R & Pixel.G + i == Find.G & Pixel.B + i == Find.B)
                {
                    return true;
                }
                if (Pixel.R - i == Find.R & Pixel.G - i == Find.G & Pixel.B - i == Find.B)
                {
                    return true;
                }
            }
            return false;

        }
        public static Boolean ComparePixelYellow(Color Pixel, int Y_Value, int Tolerance)
        {

            int yellow = ((Pixel.R + Pixel.G) / 2);
            //Debug.WriteLine(Pixel.ToString() + "|Yellow: " + yellow);
            bool ret = false;
            int rangeHigh = Y_Value + Tolerance;
            int rangeLow = Y_Value - Tolerance;
            bool inRange = (yellow <= rangeHigh & yellow >= rangeLow);
            bool outRange = !(yellow <= rangeHigh & yellow >= rangeLow);
            if (inRange)
            {
                //Debug.WriteLine("In range, Yellow Value: " + yellow);
                ret = true;
            }
            if (outRange)
            {
                //Debug.WriteLine("Out of range, Yellow Value: " + yellow);
                ret = false;
            }
            return ret;

        }
        //eleminated the Percentage for now until i get it working
        public static Color[,] GetColorArray(Rectangle rect, int X, int Y, int Width, int Height)
        {
            Color[,] colorArray = new Color[Width,Height];
            Point sPoint = PointToScreenPoint(rect, X, Y);
            int x = sPoint.X;
            int y = sPoint.Y;
            int[] stride = new int[Width];
            int[] depth = new int[Height];
            foreach (int i in stride)
            {
                foreach(int ii in depth)
                {
                    colorArray[i, ii] = GetColor(rect, x + i, y + ii);
                    //Debug.WriteLine(colorArray[i, ii]);
                }
            }

            return colorArray;
        }
        public static bool SearchPixel(Color[,] Image, Color LookingFor, int Tolerance)
        {
            foreach (Color value in Image)
            {
                if (ComparePixel(value, LookingFor, Tolerance))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool SearchPixelYellow(Color[,] Image, int Y_Value, int Tolerance)
        {
            bool cond = false;
            foreach (Color color in Image)
            {
                cond = ComparePixelYellow(color, Y_Value, Tolerance);
                if (cond == true)
                {
                    break;
                }
            }
            return cond;
        }
        public static bool SearchPixelArea(Color SearchColor, Rectangle Screen , int X, int Y, int Width, int Height)
        {
            Color[,] image = GetColorArray(Screen, X, Y, Width, Height);
            bool retValue = false;
            foreach(Color color in image)
            {
                if(color == SearchColor)
                {
                    retValue = true;
                    break;
                }
            }
            return retValue;
        }
        public static Color AveragePixelArea(Rectangle Screen, int X, int Y, int Width, int Height)
        {
            Color retColor = new Color();
            Color[,] image = new Color[Width, Height];
            Point sPoint = PointToScreenPoint(Screen, X, Y);
            int x = sPoint.X;
            int y = sPoint.Y;
            int[] stride = new int[Width];
            int[] depth = new int[Height];

            decimal alpha = 0;
            decimal red = 0;
            decimal green = 0;
            decimal blue = 0;

            int a = 0;
            int r = 0;
            int g = 0;
            int b = 0;

            int n = 0;

            foreach (int i in stride)
            {
                foreach (int ii in depth)
                {
                    image[i, ii] = GetColor(Screen, x + i, y + ii);
                    //Debug.WriteLine(image[i, ii].ToString());
                    alpha = alpha + image[i, ii].A;
                    red = red + image[i, ii].R;
                    green = green + image[i, ii].G;
                    blue = blue + image[i, ii].B;
                    n++;
                    //Debug.WriteLine(image[i, ii]);
                    //Debug.WriteLine(red + "|" + green + "|"+ blue + "|");
                }
            }

            //rounds value if non-zero

            a = (int)(alpha / n);
            r = (int)(red / n);
            g = (int)(blue / n);
            b = (int)(blue / n);
            //Debug.WriteLine(r + "|" + "|" + b + "|" + g);

            //assigns color value
            retColor = Color.FromArgb(a, r, g, b);
            //returns color value
            return retColor;
        }
        /// <summary>
        /// gives a % value for the Yellow color
        /// </summary>
        /// <param name="Pixel"></param>
        /// <returns>int: 0-100 for % value of Yellow</returns>
        public static int GetValueYellow(Color Pixel)
        {
            if (Pixel.IsEmpty)
            {
                return 0;
            }
            double C = 0;
            double Y = 0;
            double M = 0;
            double K = 0;

            if (Pixel.R == 0 | Pixel.G == 0 | Pixel.B == 0)
            {
                return 0;
            }
            if (Pixel.R == 255 | Pixel.G == 255 | Pixel.B == 255)
            {
                return 1;
            }

            C = 1.0 - (double)(Pixel.R / 255.0);
            M = 1.0 - (double)(Pixel.G / 255.0);
            Y = 1.0 - (double)(Pixel.B / 255.0);

            double minCMY = (double)Math.Min(C, Math.Min(M, Y));




            C = (C - minCMY) / (1 - minCMY);
            M = (M - minCMY) / (1 - minCMY);
            Y = (Y - minCMY) / (1 - minCMY);
            K = minCMY;

            int[] CMYK = new int[4];
            CMYK[0] = (int)Math.Round(C * 100);
            CMYK[1] = (int)Math.Round(M * 100);
            CMYK[2] = (int)Math.Round(Y * 100);
            CMYK[3] = (int)Math.Round(K * 100);

            return CMYK[2];
        }
        /// <summary>
        /// gives a % value for the Cyan color
        /// </summary>
        /// <param name="Pixel"></param>
        /// <returns>int: 0-100 for % value of Cyan</returns>
        public static int GetValueCyan(Color Pixel)
        {
            int C = 0;
            int Y = 0;
            int M = 0;
            int K = 0;

            C = 1 - (Pixel.R / 255);
            M = 1 - (Pixel.G / 255);
            Y = 1 - (Pixel.B / 255);

            var minCMY = Math.Min(C, Math.Min(M, Y));

            C = (C - minCMY) / (1 - minCMY);
            M = (M - minCMY) / (1 - minCMY);
            Y = (Y - minCMY) / (1 - minCMY);
            K = minCMY;

            int[] CMYK = new int[4];
            CMYK[0] = C * 100;
            CMYK[1] = M * 100;
            CMYK[2] = Y * 100;
            CMYK[3] = K * 100;

            return CMYK[0];
        }
        /// <summary>
        /// gives a % value for the Magenta color
        /// </summary>
        /// <param name="Pixel"></param>
        /// <returns>int: 0-100 for % value of Magenta</returns>
        public static int GetValueMagenta(Color Pixel)
        {
            int C = 0;
            int Y = 0;
            int M = 0;
            int K = 0;

            C = 1 - (Pixel.R / 255);
            M = 1 - (Pixel.G / 255);
            Y = 1 - (Pixel.B / 255);

            var minCMY = Math.Min(C, Math.Min(M, Y));

            C = (C - minCMY) / (1 - minCMY);
            M = (M - minCMY) / (1 - minCMY);
            Y = (Y - minCMY) / (1 - minCMY);
            K = minCMY;

            int[] CMYK = new int[4];
            CMYK[0] = C * 100;
            CMYK[1] = M * 100;
            CMYK[2] = Y * 100;
            CMYK[3] = K * 100;

            return CMYK[1];
        }
        public static int[] RbgToCymk(Color rbg)
        {
            int C = 0;
            int Y = 0;
            int M = 0;
            int K = 0;

            C = 1 - (rbg.R / 255);
            M = 1 - (rbg.G / 255);
            Y = 1 - (rbg.B / 255);

            var minCMY = Math.Min(C, Math.Min(M, Y));

            C = (C - minCMY) / (1 - minCMY);
            M = (M - minCMY) / (1 - minCMY);
            Y = (Y - minCMY) / (1 - minCMY);
            K = minCMY;

            int[] CMYK = new int[4];
            CMYK[0] = C * 100;
            CMYK[1] = M * 100;
            CMYK[2] = Y * 100;
            CMYK[3] = K * 100;

            return CMYK;
        }

    }
}
